using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

public class NeuralNetwork_Lucking : INeuralNetwork
{
    int L;
    List<int> LayerSize;

    List<Matrix> W; //Weights
    List<Vector> B; //Biases

    List<Matrix> vW; //Velocities for weight
    List<Vector> vB; //Velocities for weight

    double TotalError;

    public double EPS { get; set; }
    public double learningRate { get; set; }
    public double momentum { get; set; }

    public NeuralNetwork_Lucking()
    {
        LayerSize = new List<int>();
        W = new List<Matrix>();
        B = new List<Vector>();

        vW = new List<Matrix>();
        vB = new List<Vector>();

        EPS = 0.02d;
        TotalError = EPS + 1d;

        learningRate = 0.01d;
        momentum = 0d;
    }

    public void Initialize(List<int> layerSize)
    {
        LayerSize = new List<int>(layerSize);
        LayerSize.Insert(0, 0);
        L = LayerSize.Count - 1;

        //Weight and Bias
        W = new List<Matrix>(new Matrix[L + 1]);
        B = new List<Vector>(new Vector[L + 1]);

        //Velocities
        vW = new List<Matrix>(new Matrix[L + 1]);
        vB = new List<Vector>(new Vector[L + 1]);

        for (int i = 2; i <= L; i++)
        {
            W[i] = Matrix.Random(LayerSize[i], LayerSize[i - 1], new MathNet.Numerics.Distributions.ContinuousUniformDistribution(-1d, 1d));
            vW[i] = new Matrix(LayerSize[i], LayerSize[i - 1], 0d);

            double number = Random.Range(-1f, 1f);
            B[i] = new Vector(LayerSize[i], number);
            vB[i] = new Vector(LayerSize[i], 0d);
        }

        TotalError = EPS + 1d;
    }


    public void LearnIteration(List<List<double>> input, List<List<double>> output)
    {
        List<Vector> _input = new List<Vector>(new Vector[input.Count]);
        for (int i = 0; i < _input.Count; i++)
            _input[i] = Vector.Create(input[i].ToArray());

        List<Vector> _output = new List<Vector>(new Vector[output.Count]);
        for (int i = 0; i < _output.Count; i++)
            _output[i] = Vector.Create(output[i].ToArray());

        int InputSize = _input.Count;
        List<List<Vector>> D = new List<List<Vector>>(new List<Vector>[InputSize]);
        List<List<Vector>> A = new List<List<Vector>>(new List<Vector>[InputSize]);

        double OldError, NewError;
        NewError = CostFunction(_input, _output);
        OldError = NewError;

        for (int x = 0; x < InputSize; x++)
        {
            if (input[x].Count != LayerSize[1])
                throw new System.ArgumentException("Entrada N° " + x + " a aprender de dimensión diferente a la establecida en la red!");

            if (output[x].Count != LayerSize[L])
                throw new System.ArgumentException("Salida N° " + x + " a aprender de dimensión diferente a la establecida en la red!");

            List<Vector> Zx, Ax, Dx;
            Zx = new List<Vector>(new Vector[L + 1]);
            Ax = new List<Vector>(new Vector[L + 1]);
            Dx = new List<Vector>(new Vector[L + 1]);

            //Set input layer
            Ax[1] = _input[x].Clone();

            //Compute Zx and Ax
            for (int l = 2; l <= L; l++)
            {
                Zx[l] = (W[l].Clone().Multiply(Ax[l - 1].ToColumnMatrix()) + B[l].ToColumnMatrix()).GetColumnVector(0);

                Ax[l] = Zx[l].Clone();
                for (int u = 0; u < Zx[l].Length; u++)
                    Ax[l][u] = SigmoidFunction(Zx[l][u]);
            }

            //Compute Dx with backpropagation
            //Compute DLx
            Dx[L] = (Ax[L] - _output[x]);
            for (int u = 0; u < Dx[L].Length; u++)
                Dx[L][u] = Dx[L][u] * (dSigmoidFunction(Zx[L][u]));

            //Backpropagate the error
            for (int l = L - 1; l >= 2; l--)
            {
                Matrix WTM = W[l + 1].Clone();
                WTM.Transpose();
                Matrix DxM = Dx[l + 1].ToColumnMatrix();
                WTM = WTM * DxM;
                Vector WTMV = WTM.GetColumnVector(0);
                Vector ZxldS = Zx[l].Clone();
                for (int u = 0; u < ZxldS.Length; u++)
                    ZxldS[u] = dSigmoidFunction(ZxldS[u]);

                Dx[l] = WTMV.Clone();
                for (int u = 0; u < Dx[l].Length; u++)
                    Dx[l][u] = WTMV[u] * ZxldS[u];
            }

            //Add Ax and Dx to their vectors
            A[x] = new List<Vector>(Ax);
            D[x] = new List<Vector>(Dx);

        }

        List<Matrix> AuxW = new List<Matrix>(W);
        List<Vector> AuxB = new List<Vector>(B);

        //Gradient descent
        for (int l = L; l >= 2; l--)
        {
            Matrix dW = new Matrix(W[l].RowCount, W[l].ColumnCount, 0d);
            for (int x = 0; x < InputSize; x++)
            {
                Matrix aux1 = new Matrix(D[x][l].Length, 1);
                for (int pos = 0; pos < D[x][l].Length; pos++)
                    aux1[pos, 0] = D[x][l][pos];

                Matrix aux2 = new Matrix(1, A[x][l - 1].Length);
                for (int pos = 0; pos < A[x][l - 1].Length; pos++)
                    aux2[0, pos] = A[x][l - 1][pos];

                Matrix _dW = aux1.Multiply(aux2); //?
                dW += _dW;
            }

            Vector dB = new Vector(D[0][l].Length, 0d);
            for (int x = 0; x < InputSize; x++)
                dB += D[x][l].Clone();

            //Update Weights with momentum
            for (int i = 0; i < W.Count; i++)
            {
                vW[l] = (momentum * vW[l]) - (learningRate * dW);
                AuxW[l] = W[l] + vW[l];
            }

            //Update Biases with momentum
            vB[l] = (momentum * vB[l]) - (learningRate * dB);
            AuxB[l] = B[l] + vB[l];

        }

        W = new List<Matrix>(AuxW);
        B = new List<Vector>(AuxB);   

        NewError = CostFunction(_input, _output);
        double ErrorChange = NewError - OldError;
        TotalError = NewError;

    }

    public void Learn(List<List<double>> input, List<List<double>> output)
    {
        List<Vector> _input = new List<Vector>(new Vector[input.Count]);
        for (int i = 0; i < _input.Count; i++)
            _input[i] = Vector.Create(input[i].ToArray());

        List<Vector> _output = new List<Vector>(new Vector[output.Count]);
        for (int i = 0; i < _output.Count; i++)
            _output[i] = Vector.Create(output[i].ToArray());

        int InputSize = _input.Count;
        List<List<Vector>> D = new List<List<Vector>>(new List<Vector>[InputSize]);
        List<List<Vector>> A = new List<List<Vector>>(new List<Vector>[InputSize]);

        double OldError, NewError;

        TotalError = EPS + 1d;

        int maxLoops = 10000;
        int counter = 0;

        NewError = CostFunction(_input, _output);
        while (TotalError > EPS)
        {
            if (counter++ > maxLoops)
                break;

            OldError = NewError;

            for (int x = 0; x < InputSize; x++)
            {
                if (input[x].Count != LayerSize[1])
                    throw new System.ArgumentException("Entrada N° " + x + " a aprender de dimensión diferente a la establecida en la red!");

                if (output[x].Count != LayerSize[L])
                    throw new System.ArgumentException("Salida N° " + x + " a aprender de dimensión diferente a la establecida en la red!");

                List<Vector> Zx, Ax, Dx;
                Zx = new List<Vector>(new Vector[L + 1]);
                Ax = new List<Vector>(new Vector[L + 1]);
                Dx = new List<Vector>(new Vector[L + 1]);

                //Set input layer
                Ax[1] = _input[x].Clone();

                //Compute Zx and Ax
                for (int l = 2; l <= L; l++)
                {
                    Zx[l] = (W[l].Clone().Multiply(Ax[l - 1].ToColumnMatrix()) + B[l].ToColumnMatrix()).GetColumnVector(0);

                    Ax[l] = Zx[l].Clone();
                    for (int u = 0; u < Zx[l].Length; u++)
                        Ax[l][u] = SigmoidFunction(Zx[l][u]);
                }

                //Compute Dx with backpropagation
                //Compute DLx
                Dx[L] = (Ax[L] - _output[x]);
                for (int u = 0; u < Dx[L].Length; u++)
                    Dx[L][u] = Dx[L][u] * (dSigmoidFunction(Zx[L][u]));

                //Backpropagate the error
                for (int l = L - 1; l >= 2; l--)
                {
                    Matrix WTM = W[l + 1].Clone();
                    WTM.Transpose();
                    Matrix DxM = Dx[l + 1].ToColumnMatrix();
                    WTM = WTM * DxM;
                    Vector WTMV = WTM.GetColumnVector(0);
                    Vector ZxldS = Zx[l].Clone();
                    for (int u = 0; u < ZxldS.Length; u++)
                        ZxldS[u] = dSigmoidFunction(ZxldS[u]);

                    Dx[l] = WTMV.Clone();
                    for (int u = 0; u < Dx[l].Length; u++)
                        Dx[l][u] = WTMV[u] * ZxldS[u];
                }

                //Add Ax and Dx to their vectors
                A[x] = new List<Vector>(Ax);
                D[x] = new List<Vector>(Dx);

            }

            List<Matrix> AuxW = new List<Matrix>(W);
            List<Vector> AuxB = new List<Vector>(B);

            //Gradient descent
            for (int l = L; l >= 2; l--)
            {
                Matrix dW = new Matrix(W[l].RowCount, W[l].ColumnCount, 0d);
                for (int x = 0; x < InputSize; x++)
                {
                    Matrix aux1 = new Matrix(D[x][l].Length, 1);
                    for (int pos = 0; pos < D[x][l].Length; pos++)
                        aux1[pos, 0] = D[x][l][pos];

                    Matrix aux2 = new Matrix(1, A[x][l - 1].Length);
                    for (int pos = 0; pos < A[x][l - 1].Length; pos++)
                        aux2[0, pos] = A[x][l - 1][pos];

                    Matrix _dW = aux1.Multiply(aux2); //?
                    dW += _dW;
                }

                Vector dB = new Vector(D[0][l].Length, 0d);
                for (int x = 0; x < InputSize; x++)
                    dB += D[x][l].Clone();

                //Update Weights with momentum
                for (int i = 0; i < W.Count; i++)
                {
                    vW[l] = (momentum * vW[l]) - (learningRate * dW);
                    AuxW[l] = W[l] + vW[l];
                }

                //Update Biases with momentum
                vB[l] = (momentum * vB[l]) - (learningRate * dB);
                AuxB[l] = B[l] + vB[l];

            }

            W = new List<Matrix>(AuxW);
            B = new List<Vector>(AuxB);

            NewError = CostFunction(_input, _output);
            double ErrorChange = NewError - OldError;
            Debug.Log(NewError);
            TotalError = NewError;
        }

    }

    public List<double> Predict(List<double> input)
    {
        if (input.Count != LayerSize[1])
            throw new System.ArgumentException("Entrada a predecir de dimensión diferente a la establecida en la red!");

        List<Vector> Z, A;
        Z = new List<Vector>(new Vector[L + 1]);
        A = new List<Vector>(new Vector[L + 1]);

        //Set input layer
        A[1] = Vector.Create(input.ToArray());
        for (int l = 2; l <= L; l++)
        {
            Z[l] = (W[l].Clone() * (A[l - 1].ToColumnMatrix()) + B[l].ToColumnMatrix()).GetColumnVector(0);

            A[l] = Z[l].Clone();
            for (int u = 0; u < Z[l].Length; u++)
                A[l][u] = SigmoidFunction(Z[l][u]);
        }

        List<double> result = new List<double>(A[L].CopyToArray());
        return result;
    }

    private Vector privatePredict(Vector input)
    {
        List<Vector> Z, A;
        Z = new List<Vector>(new Vector[L + 1]);
        A = new List<Vector>(new Vector[L + 1]);

        //Set input layer
        A[1] = input.Clone();
        for (int l = 2; l <= L; l++)
        {
            Z[l] = (W[l].Clone().Multiply(A[l - 1].ToColumnMatrix()) + B[l].ToColumnMatrix()).GetColumnVector(0);

            A[l] = Z[l].Clone();
            for (int u = 0; u < Z[l].Length; u++)
                A[l][u] = SigmoidFunction(Z[l][u]);
        }

        return A[L].Clone();
    }

    private double CostFunction(Vector input, Vector desiredOutput)
    {
        Vector ActualOutput = privatePredict(input);
        Vector Result = desiredOutput - ActualOutput;
        for (int u = 0; u < Result.Length; u++)
            Result[u] = (Result[u] * Result[u]) / 2d;

        double Cost = 0d;

        for (int i = 0; i < Result.Length; i++)
            Cost += Result[i];

        return Cost;
    }

    private double CostFunction(List<Vector> input, List<Vector> desiredOutput)
    {
        double InputSize = input.Count;
        double OutputSize = desiredOutput.Count;
        double Cost = 0d;

        //Quadratic cost
        Vector TotalResult = new Vector(desiredOutput[0].Length, 0d);
        for (int j = 0; j < InputSize; j++)
        {
            Vector ActualOutput = privatePredict(input[j]);
            Vector Result = desiredOutput[j] - ActualOutput;
            for (int u = 0; u < Result.Length; u++)
                Result[u] = (Result[u] * Result[u]) / 2d;

            TotalResult = TotalResult + Result;
        }
        for (int i = 0; i < TotalResult.Length; i++)
            Cost += TotalResult[i];

        return Cost;
    }

    public double SigmoidFunction(double x)
    {
        return 1d / (1d + Mathf.Exp((float)-x));
    }

    public double dSigmoidFunction(double x)
    {
        double y = SigmoidFunction(x);
        return (1d - y) * y;
    }

}
