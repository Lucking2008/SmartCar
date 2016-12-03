#pragma once

#include <vector>
using namespace std;

typedef vector<int> vi;
typedef vector<double> vd;
typedef vector<vd> vvd;

class INeuralNetwork
{
public:
	virtual ~INeuralNetwork() {}

	virtual void Initialize(vi LayerSize) = 0;
	
	virtual void Learn(vvd Input, vvd Output, double LearningRate) = 0;
	virtual void LearnIteration(vd Input, vd Output, double LearningRate) = 0; 
	
	virtual vector<double> Predict(vector<double> Input) = 0;

};

