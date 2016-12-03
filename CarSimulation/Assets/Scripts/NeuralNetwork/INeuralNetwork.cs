using UnityEngine;
using System.Collections.Generic;

public interface INeuralNetwork
{
    void Initialize(List<int> size);
    void LearnIteration(List<List<double>> input, List<List<double>> output);
    void Learn(List<List<double>> input, List<List<double>> output);
    List<double> Predict(List<double> input);
    double SigmoidFunction(double x);
}
