using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using System;

public class NeuralNetwork_Kuro : INeuralNetwork
{

    //memory
    public Double[,,] w { get; set; }
    public Double[,,] p { get; set; }
    public Double[,] o { get; set; }
    public Double[,] e { get; set; }
    public Int32[] l { get; set; }


    //learning parameters
    public Double alpha { get; set; }
    public Double beta { get; set; }

    public void Initialize(List<int> layerSize)
    {
        Initialize(layerSize, 0.35, 0.45);
    }

    public void Initialize(List<int> layerSize, double alpha = 0.3, double beta = 0.4)
    {
        this.alpha = alpha; this.beta = beta;
        this.l = layerSize.ToArray();
        w = new Double[layerSize.Count, getMax(layerSize) + 1, getMax(layerSize) + 1];
        p = new Double[layerSize.Count, getMax(layerSize) + 1, getMax(layerSize) + 1];
        System.Random r = new System.Random();

        for (int i = 0; i < layerSize.Count - 1; i++)
            for (int k = 0; k < l[i] + 1; k++)
                for (int m = 0; m < l[i + 1]; m++)
                {
                    w[i, k, m] = (r.NextDouble() - 0.5) * 2;
                    p[i, k, m] = 0;
                }
        o = new Double[l.Length, getMax(l) + 1];
        e = new Double[l.Length, getMax(l) + 1];
    }

    public double SigmoidFunction(double x)
    {
        return sigmoid(x);
    }

    public double sigmoid(double t)
    {
        return 1 / (1 + Math.Pow(Math.E, -1 * t));
    }


    public List<double> Predict(List<double> x)
    {
        for (int i = 0; i < l.Length; i++)
            for (int k = 0; k < l[i] + 1; k++)
                o[i, k] = l[i] == k ? 1 : 0;

        for (int i = 0; i < x.Count; i++)
            o[0, i] = x[i];

        for (int i = 0; i < l.Length - 1; i++)
        {
            for (int k = 0; k < l[i] + 1; k++)
                for (int m = 0; m < l[i + 1]; m++)
                    o[i + 1, m] += w[i, k, m] * o[i, k];

            for (int k = 0; k < l[i + 1]; k++)
                o[i + 1, k] = sigmoid(o[i + 1, k]);
        }

        List<Double> result = new List<double>();
        for (int i = 0; i < l[l.Length - 1]; i++)
            result.Add(o[l.Length - 1, i]);

        return result;
    }

    public void LearnIteration(List<List<double>> input, List<List<double>> output)
    {
        LearnOneExample(input[0], output[0]);
    }

    public void LearnOneExample(List<double> x, List<double> y)
    {
        for (int i = 0; i < l.Length; i++)
            for (int k = 0; k < l[i] + 1; k++)
                e[i, k] = 0;
        Predict(x);


        //get error for output layer
        int s = l.Length - 1;
        for (int i = 0; i < l[s]; i++)
            e[s, i] = o[s, i] * (1 - o[s, i]) * (y[i] - o[s, i]);


        // back prop the error
        for (int i = l.Length - 2; i > 0; i--)
        {

            for (int k = 0; k < l[i] + 1; k++)
                for (int m = 0; m < l[i + 1]; m++)
                    e[i, k] += w[i, k, m] * e[i + 1, m];

            for (int k = 0; k < l[i] + 1; k++)
            {
                e[i, k] = o[i, k] * (1 - o[i, k]) * e[i, k];
            }
        }
        //Apply momentum
        for (int i = 0; i < l.Length - 1; i++)
            for (int k = 0; k < l[i] + 1; k++)
                for (int m = 0; m < l[i + 1]; m++)
                {
                    w[i, k, m] += alpha * p[i, k, m];

                }

        //Adjust weights
        for (int i = 0; i < l.Length - 1; i++)
            for (int k = 0; k < l[i] + 1; k++)
                for (int m = 0; m < l[i + 1]; m++)
                {
                    p[i, k, m] = beta * e[i + 1, m] * o[i, k];
                    w[i, k, m] += p[i, k, m];
                }
    }

    public void Learn(List<List<double>> input, List<List<double>> output)
    {
        LearnVectorOfExample(input, output);
    }

    public int LearnVectorOfExample(List<List<double>> lstX, List<List<double>> lstY, double maxError = 0.01)
    {
        Double error = 1;
        Int32 it = 0;
        while (error >= maxError)
        {
            error = 0;
            it++;
            for (int i = 0; i < lstY.Count; i++)
            {
                LearnOneExample(lstX[i], lstY[i]);
                error += SquareError(lstY[i]);

            }
            error /= lstY.Count;
            //Console.WriteLine("error: " + error);

            if (it > 3000)
            {
                // Console.WriteLine("Failed");
                return it;
            }
        }
        // Console.WriteLine("Success in "+it+" iterations:");

        for (int i = 0; i < lstY.Count; i++)
        {
            Predict(lstX[i]);
            //printOutput();
        }
        return it;
    }

    public Double SquareError(List<Double> y)
    {
        Double sum = 0;
        Int32 s = l.Length - 1;
        for (int i = 0; i < y.Count; i++)
        {
            sum += (o[s, i] - y[i]) * (o[s, i] - y[i]) / 2;
        }
        return sum;
    }
    public void printWeights()
    {
        for (int i = 0; i < l.Length - 1; i++)
        {
            for (int k = 0; k < l[i] + 1; k++)
            {
                for (int m = 0; m < l[i + 1]; m++)
                {
                    Console.Write(" " + w[i, k, m].ToString("0.00"));
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
    public void printOutput()
    {
        int s = l.Length - 1;
        for (int i = 0; i < l[s]; i++)
        {
            Console.Write(" " + o[s, i]);
        }
        Console.WriteLine("");
    }

    private int getMax(List<int> l)
    {
        int value = l[0];
        foreach (var item in l)
            if (item > value)
                value = item;
        return value;
    }

    private int getMax(int[] l)
    {
        int value = l[0];
        foreach (var item in l)
            if (item > value)
                value = item;
        return value;
    }
}
