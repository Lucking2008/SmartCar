#include "NeuralNetwork.h"


NeuralNetwork::NeuralNetwork()
{
	srand(time(0));
	RandomEngine = mt19937(time(0));
}


NeuralNetwork::~NeuralNetwork()
{
}

void NeuralNetwork::Initialize(vector<int> pLayerSize)
{
	LayerSize = pLayerSize;
	LayerSize.insert(LayerSize.begin(), 0);
	L = LayerSize.size() - 1;

	//Weight and Bias
	W = vector<MatrixXd>(L + 1);
	B = vector<VectorXd>(L + 1);

	for (int i = 2; i <= L; i++) {
		W[i] = MatrixXd(LayerSize[i], LayerSize[i - 1]);
		W[i].setRandom();
		uniform_real_distribution<double> dst(0, 1.0);
		double rnd = dst(RandomEngine);
		B[i] = VectorXd::Constant(LayerSize[i], rnd);
	}
}

VectorXd NeuralNetwork::Predict(VectorXd Input)
{
	vector<VectorXd> Z, A;
	Z = vector<VectorXd>(L + 1);
	A = vector<VectorXd>(L + 1);

	//Set input layer
	A[1] = Input;
	for (int l = 2; l <= L; l++) {
		Z[l] = W[l] * A[l - 1] + B[l];
		A[l] = Z[l].unaryExpr(&SigmoidFunction);
	}
	return A[L];
}

double NeuralNetwork::SigmoidFunction(double X)
{
	return 1.0 / (1.0 + exp(-X));
}

double NeuralNetwork::dSigmoidFunction(double X)
{
	double y = SigmoidFunction(X);
	return (1.0 - y) * y;
}

double NeuralNetwork::Square(double X)
{
	double y = X * X;
	return y;
}

void  NeuralNetwork::LearnIteration(vector<VectorXd> Input, vector<VectorXd> Output, double LearningRate)
{
	int InputSize = Input.size();
	vector<vector<VectorXd>> D(InputSize);
	vector<vector<VectorXd>> A(InputSize);

	double Error = 0.0;

	for (int x = 0; x < InputSize; x++) {
		vector<VectorXd> Zx, Ax, Dx;
		Zx = vector<VectorXd>(L + 1);
		Ax = vector<VectorXd>(L + 1);
		Dx = vector<VectorXd>(L + 1);

		//Set input layer
		Ax[1] = Input[x];

		//Compute Zx and Ax
		for (int l = 2; l <= L; l++) {
			Zx[l] = W[l] * Ax[l - 1] + B[l];
			Ax[l] = Zx[l].unaryExpr(&SigmoidFunction);
		}

		//Compute Dx with backpropagation
		//Compute DLx
		Dx[L] = (Ax[L] - Output[x]).cwiseProduct(Zx[L].unaryExpr(&dSigmoidFunction));

		//Backpropagate the error
		for (int l = L - 1; l >= 2; l--)
			Dx[l] = ((W[l + 1].transpose())*Dx[l + 1]).cwiseProduct(Zx[l].unaryExpr(&dSigmoidFunction));

		//Add Ax and Dx to their vectors
		A[x] = Ax;
		D[x] = Dx;

	}

	vector<MatrixXd> AuxW = W;
	vector<VectorXd> AuxB = B;

	//Gradient descent
	for (int l = L; l >= 2; l--) {
		MatrixXd dW = MatrixXd::Zero(W[l].rows(), W[l].cols());
		for (int x = 0; x < InputSize; x++) {
			MatrixXd _dW = D[x][l] * (A[x][l - 1].transpose());
			dW += _dW;
		}

		VectorXd dB = VectorXd::Zero(D[0][l].size());
		for (int x = 0; x < InputSize; x++)
			dB += D[x][l];

		//Update Weights
		for (int i = 0; i < W.size(); i++)
			AuxW[l] = W[l] - ((LearningRate / InputSize) * dW);

		//Update Biases
		AuxB[l] = B[l] - ((LearningRate / InputSize) * dB);
	}

	W = AuxW;
	B = AuxB;

	Error = CostFunction(Input, Output);
	cout << Error << endl;
}

void NeuralNetwork::Learn(vector<VectorXd> Input, vector<VectorXd> Output, double LearningRate)
{
	int InputSize = Input.size();
	vector<vector<VectorXd>> D(InputSize);
	vector<vector<VectorXd>> A(InputSize);

	double Error, EPS;

	EPS = 0.02;
	Error = CostFunction(Input, Output);

	OutputStream = ofstream("Errors.csv");
	OutputStream << Error << endl;

	while (Error > EPS) {

		for (int x = 0; x < InputSize; x++) {
			vector<VectorXd> Zx, Ax, Dx;
			Zx = vector<VectorXd>(L + 1);
			Ax = vector<VectorXd>(L + 1);
			Dx = vector<VectorXd>(L + 1);

			//Set input layer
			Ax[1] = Input[x];

			//Compute Zx and Ax
			for (int l = 2; l <= L; l++) {
				Zx[l] = W[l] * Ax[l - 1] + B[l];
				Ax[l] = Zx[l].unaryExpr(&SigmoidFunction);
			}

			//Compute Dx with backpropagation
			//Compute DLx
			Dx[L] = (Ax[L] - Output[x]).cwiseProduct(Zx[L].unaryExpr(&dSigmoidFunction));

			//Backpropagate the error
			for (int l = L - 1; l >= 2; l--)
				Dx[l] = ((W[l + 1].transpose())*Dx[l + 1]).cwiseProduct(Zx[l].unaryExpr(&dSigmoidFunction));

			//Add Ax and Dx to their vectors
			A[x] = Ax;
			D[x] = Dx;

		}

		vector<MatrixXd> AuxW = W;
		vector<VectorXd> AuxB = B;

		//Gradient descent
		for (int l = L; l >= 2; l--) {
			MatrixXd dW = MatrixXd::Zero(W[l].rows(), W[l].cols());
			for (int x = 0; x < InputSize; x++) {
				MatrixXd _dW = D[x][l] * (A[x][l - 1].transpose());
				dW += _dW;
			}

			VectorXd dB = VectorXd::Zero(D[0][l].size());
			for (int x = 0; x < InputSize; x++)
				dB += D[x][l];

			//Update Weights
			for(int i = 0; i < W.size(); i++)
				AuxW[l] = W[l] - ((LearningRate / InputSize) * dW);

			//Update Biases
			AuxB[l] = B[l] - ((LearningRate / InputSize) * dB);
		}

		W = AuxW;
		B = AuxB;

		Error = CostFunction(Input, Output);
		OutputStream << Error << endl;
	}

	OutputStream.close();
}

VectorXd NeuralNetwork::CostFunction(VectorXd Input, VectorXd DesiredOutput)
{
	VectorXd ActualOutput = Predict(Input);
	VectorXd Result = ((DesiredOutput - ActualOutput).unaryExpr(&Square)) / 2.0;
	return Result;
}

double NeuralNetwork::CostFunction(vector<VectorXd> Input, vector<VectorXd> DesiredOutput)
{
	double InputSize = Input.size();
	double OutputSize = DesiredOutput.size();
	double Cost = 0.0;

	//Quadratic cost
	VectorXd TotalResult = VectorXd::Zero(DesiredOutput[0].size());
	for (int j = 0; j < InputSize; j++) {
		VectorXd ActualOutput = Predict(Input[j]);
		VectorXd Result = ((DesiredOutput[j] - ActualOutput).unaryExpr(&Square));
		TotalResult = TotalResult + Result;
	}
	for (int i = 0; i < TotalResult.size(); i++)
		Cost += TotalResult(i);

	return Cost;
}


VectorXd NeuralNetwork::dCostFunction(VectorXd Input, VectorXd DesiredOutput)
{
	VectorXd ActualOutput = Predict(Input);
	VectorXd Result = (DesiredOutput - ActualOutput).cwiseAbs();
	return Result;
}

VectorXd NeuralNetwork::dCostFunction(vector<VectorXd> Input, vector<VectorXd> DesiredOutput)
{
	double InputSize = Input.size();
	VectorXd TotalResult = VectorXd::Zero(DesiredOutput[0].size());
	for (int j = 0; j < Input.size(); j++) {
		VectorXd ActualOutput = Predict(Input[j]);
		VectorXd Result = ((DesiredOutput[j] - ActualOutput).cwiseAbs());
		TotalResult = TotalResult + Result;
	}

	TotalResult = TotalResult / InputSize;
	return TotalResult;
}

void NeuralNetwork::Read(string File)
{
	ifstream Input(File);
	if (Input.is_open()) {
		Input >> L;
		
		LayerSize = vector<int>(L + 1, 0);
		W = vector<MatrixXd>(L + 1);
		B = vector<VectorXd>(L + 1);

		for (int l = 1; l <= L; l++)
			Input >> LayerSize[l];

		double num = 0.0;
		for (int l = 2; l <= L; l++) {
			W[l] = MatrixXd(LayerSize[l], LayerSize[l - 1]);
			for (int i = 0; i < W[l].rows(); i++)
				for (int j = 0; j < W[l].cols(); j++) {		
					Input >> num;
					W[l](i, j) = num;
				}

			int holi = 1;
			B[l] = VectorXd(LayerSize[l]);
			for (int i = 0; i < B[l].size(); i++) {
				Input >> num;
				B[l](i) = num;
			}
		}
	}
}

void NeuralNetwork::Write(string File)
{
	ofstream Output(File);
	if (Output.is_open()) {
		Output << L << endl;
		for (int i = 1; i < LayerSize.size(); i++) {
			Output << LayerSize[i];
			i != L ? Output << " " : Output << endl;
		}

		for (int i = 2; i < W.size(); i++) {
			Output << W[i] << endl;
			Output << B[i] << endl;
		}

		Output.close();
	}
}
