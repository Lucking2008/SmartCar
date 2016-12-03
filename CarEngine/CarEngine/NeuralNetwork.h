#pragma once
//STD
#include <iostream>
#include <fstream>
#include <vector>
#include <cmath>
#include <ctime>
#include <random>
#include <limits>

//Eigen library
#include <Dense>

using namespace std;
using namespace Eigen;


class NeuralNetwork
{
private:
	mt19937 RandomEngine;

	ifstream InputStream;
	ofstream OutputStream;

	int L;
	vector<int> LayerSize;

	vector<MatrixXd> W; //Weights
	vector<VectorXd> B; //Biases

public:
	NeuralNetwork();
	~NeuralNetwork();

	void Initialize(vector<int> pLayerSize);
	
	void LearnIteration(vector<VectorXd> Input, vector<VectorXd> Output, double LearningRate = 1.0);
	void Learn(vector<VectorXd> Input, vector<VectorXd> Output, double LearningRate = 1.0);
	
	VectorXd Predict(VectorXd Input);

	static double SigmoidFunction(double X);
	static double dSigmoidFunction(double X);
	static double Square(double X);

	VectorXd CostFunction(VectorXd Input, VectorXd DesiredOutput);
	double CostFunction(vector<VectorXd> Input, vector<VectorXd> DesiredOutput);

	VectorXd dCostFunction(VectorXd Input, VectorXd DesiredOutput);
	VectorXd dCostFunction(vector<VectorXd> Input, vector<VectorXd> DesiredOutput);

	void Read(string File);
	void Write(string File);
};

