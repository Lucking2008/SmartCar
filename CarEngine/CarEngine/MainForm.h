#pragma once

#include "NeuralNetwork.h"
#include "Car.h"
#include <memory>

namespace CarEngine {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Resumen de MainForm
	/// </summary>
	public ref class MainForm : public System::Windows::Forms::Form
	{
	public:
		MainForm(void)
		{
			InitializeComponent();
			//
			//TODO: agregar código de constructor aquí
			//

			SmartCar = new Car();
			NN = new NeuralNetwork();
		}

	protected:
		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		~MainForm()
		{	
			if (components)
			{
				delete components;
			}

			if (SmartCar != nullptr)
			{
				delete SmartCar;
				SmartCar = nullptr;
			}

			if (NN != nullptr)
			{
				delete NN;
				NN = nullptr;
			}

		}
	private: System::ComponentModel::IContainer^  components;
	protected:
	private: System::Windows::Forms::Timer^  Draw;

	private:
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>

		NeuralNetwork* NN;
	private: System::Windows::Forms::Timer^  Logic;
			 Car* SmartCar;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			this->Draw = (gcnew System::Windows::Forms::Timer(this->components));
			this->Logic = (gcnew System::Windows::Forms::Timer(this->components));
			this->SuspendLayout();
			// 
			// Draw
			// 
			this->Draw->Enabled = true;
			this->Draw->Interval = 20;
			this->Draw->Tick += gcnew System::EventHandler(this, &MainForm::Draw_Tick);
			// 
			// Logic
			// 
			this->Logic->Enabled = true;
			this->Logic->Interval = 20;
			this->Logic->Tick += gcnew System::EventHandler(this, &MainForm::Logic_Tick);
			// 
			// MainForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(284, 261);
			this->Name = L"MainForm";
			this->Text = L"MainForm";
			this->Load += gcnew System::EventHandler(this, &MainForm::MainForm_Load);
			this->KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &MainForm::MainForm_KeyDown);
			this->ResumeLayout(false);

		}
#pragma endregion
	private: System::Void MainForm_Load(System::Object^  sender, System::EventArgs^  e) {
		NN->Initialize({ 2, 2, 1 });
	}

	private: System::Void Draw_Tick(System::Object^  sender, System::EventArgs^  e) {

		Graphics^ g = this->CreateGraphics();
		g->Clear(Color::Transparent);

		Polygon2D& body = SmartCar->body;
		size_t bodySize = body.getSize();

		for (size_t i = 0; i < bodySize; i++)
		{
			Point2D p1 = body[i % bodySize];
			Point2D p2 = body[(i + 1) % bodySize];

			g->DrawLine(Pens::Green, (float) p1.getX(), this->Height - 50 - (float) p1.getY(), (float) p2.getX(), this->Height - 50 - (float) p2.getY());
		}

		delete g;

	}
	private: System::Void MainForm_KeyDown(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e) {

		double angle = 10 * 3.14 / 180;

		if (e->KeyCode == Keys::Right)
			SmartCar->Rotate(-angle);
		if (e->KeyCode == Keys::Left)
			SmartCar->Rotate(angle);

	}
	private: System::Void Logic_Tick(System::Object^  sender, System::EventArgs^  e) {

		vector<VectorXd> inputs;
		vector<VectorXd> outputs;

		VectorXd input(2);
		VectorXd output(1);

		input << 0, 0;	output << 0;
		inputs.push_back(input); outputs.push_back(output);

		input << 0, 1;	output << 1;
		inputs.push_back(input); outputs.push_back(output);

		input << 1, 0;	output << 1;
		inputs.push_back(input); outputs.push_back(output);

		input << 1, 1;	output << 0;
		inputs.push_back(input); outputs.push_back(output);

		NN->LearnIteration(inputs, outputs);

	}
};
}
