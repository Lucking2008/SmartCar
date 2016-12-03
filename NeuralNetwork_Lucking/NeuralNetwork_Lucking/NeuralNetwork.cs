using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using System.IO;

namespace NeuralNetwork_Lucking
{
    class NeuralNetwork
    {

        int L;
        List<int> LayerSize;

        List<Matrix<double>> W; //Weights
        List<Vector<double>> B; //Biases

        List<Matrix<double>> vW; //Velocities for weight
        List<Vector<double>> vB; //Velocities for weight

        double TotalError;

        public double EPS { get; set; }
        public double learningRate { get; set; }
        public double momentum { get; set; }

        public NeuralNetwork()
        {
            LayerSize = new List<int>();
            W = new List<Matrix<double>>();
            B = new List<Vector<double>>();

            vW = new List<Matrix<double>>();
            vB = new List<Vector<double>>();

            EPS = 0.02d;
            TotalError = EPS + 1d;

            learningRate = 1d;
            momentum = 0d;
        }

        public void Initialize(List<int> layerSize)
        {
            LayerSize = layerSize;
            LayerSize.Insert(0, 0);
            L = LayerSize.Count - 1;

            //Weight and Bias
            W = new List<Matrix<double>>(new Matrix<double>[L + 1]);
            B = new List<Vector<double>>(new Vector<double>[L + 1]);

            //Velocities
            vW = new List<Matrix<double>>(new Matrix<double>[L + 1]);
            vB = new List<Vector<double>>(new Vector<double>[L + 1]);

            for (int i = 2; i <= L; i++)
            {
                W[i] = Matrix<double>.Build.Random(LayerSize[i], LayerSize[i - 1], new MathNet.Numerics.Distributions.ContinuousUniform(0d, 1d));
                vW[i] = Matrix<double>.Build.Dense(LayerSize[i], LayerSize[i - 1], 0d);

                Random random = new Random();
                double number = random.NextDouble();
                B[i] = Vector<double>.Build.Dense(LayerSize[i], number);
                vB[i] = Vector<double>.Build.Dense(LayerSize[i], 0d);
            }

            TotalError = EPS + 1d;
        }


        public void LearnIteration(List<List<double>> input, List<List<double>> output)
        {
            //Convert List<List<double>> to List<Vector<double>>
            List<Vector<double>> _input = new List<Vector<double>>(new Vector<double>[input.Count]);
            for (int i = 0; i < _input.Count; i++)
                _input[i] = Vector<double>.Build.DenseOfEnumerable(input[i]);

            List<Vector<double>> _output = new List<Vector<double>>(new Vector<double>[output.Count]);
            for (int i = 0; i < _output.Count; i++)
                _output[i] = Vector<double>.Build.DenseOfEnumerable(output[i]);

            int InputSize = _input.Count;
            List<List<Vector<double>>> D = new List<List<Vector<double>>>(new List<Vector<double>>[InputSize]);
            List<List<Vector<double>>> A = new List<List<Vector<double>>>(new List<Vector<double>>[InputSize]);

            double OldError, NewError;

            OldError = CostFunction(_input, _output);

            for (int x = 0; x < InputSize; x++)
            {
                List<Vector<double>> Zx, Ax, Dx;
                Zx = new List<Vector<double>>(new Vector<double>[L + 1]);
                Ax = new List<Vector<double>>(new Vector<double>[L + 1]);
                Dx = new List<Vector<double>>(new Vector<double>[L + 1]);

                //Set input layer
                Ax[1] = _input[x];

                //Compute Zx and Ax
                for (int l = 2; l <= L; l++)
                {
                    Zx[l] = W[l] * Ax[l - 1] + B[l];
                    Ax[l] = Zx[l].Map(y => SigmoidFunction(y), Zeros.Include);
                }

                //Compute Dx with backpropagation
                //Compute DLx
                Dx[L] = (Ax[L] - _output[x]).PointwiseMultiply(Zx[L].Map(y => dSigmoidFunction(y), Zeros.Include));

                //Backpropagate the error
                for (int l = L - 1; l >= 2; l--)
                    Dx[l] = ((W[l + 1].Transpose()) * Dx[l + 1]).PointwiseMultiply(Zx[l].Map(y => dSigmoidFunction(y), Zeros.Include));

                //Add Ax and Dx to their vectors
                A[x] = Ax;
                D[x] = Dx;
            }

            List<Matrix<double>> AuxW = W;
            List<Vector<double>> AuxB = B;

            //Gradient descent
            for (int l = L; l >= 2; l--)
            {
                Matrix<double> dW = Matrix<double>.Build.Dense(W[l].RowCount, W[l].ColumnCount, 0d);
                for (int x = 0; x < InputSize; x++)
                {
                    Matrix<double> aux1 = Matrix<double>.Build.Dense(D[x][l].Count, 1);
                    for (int pos = 0; pos < D[x][l].Count; pos++)
                        aux1[pos, 0] = D[x][l][pos];

                    Matrix<double> aux2 = Matrix<double>.Build.Dense(1, A[x][l - 1].Count);
                    for (int pos = 0; pos < A[x][l - 1].Count; pos++)
                        aux2[0, pos] = A[x][l - 1][pos];

                    Matrix<double> _dW = aux1 * aux2;
                    dW += _dW;
                }

                Vector<double> dB = Vector<double>.Build.Dense(D[0][l].Count, 0d);
                for (int x = 0; x < InputSize; x++)
                    dB += D[x][l];

                //Update Weights
                for (int i = 0; i < W.Count(); i++)
                {
                    //Gradient descent with momentum
                    vW[l] = (momentum * vW[l]) - (learningRate * dW);
                    AuxW[l] = W[l] + vW[l];

                    //Simple Gradient Descent
                    //AuxW[l] = W[l] - ((learningRate / InputSize) * dW);
                }

                //Update Biases
                vB[l] = (momentum * vB[l]) - (learningRate * dB);
                AuxB[l] = B[l] + vB[l];

                //Simple Gradient Descent
                //AuxB[l] = B[l] - ((learningRate / InputSize) * dB);
            }

            W = AuxW;
            B = AuxB;

            NewError = CostFunction(_input, _output);
            TotalError = NewError;

        }

        public void Learn(List<List<double>> input, List<List<double>> output)
        {
            //Convert List<List<double>> to List<Vector<double>>
            List<Vector<double>> _input = new List<Vector<double>>(new Vector<double>[input.Count]);
            for (int i = 0; i < _input.Count(); i++)
                _input[i] = Vector<double>.Build.DenseOfEnumerable(input[i]);

            List<Vector<double>> _output = new List<Vector<double>>(new Vector<double>[output.Count]);
            for (int i = 0; i < _output.Count(); i++)
                _output[i] = Vector<double>.Build.DenseOfEnumerable(output[i]);

            int InputSize = _input.Count;
            List<List<Vector<double>>> D = new List<List<Vector<double>>>(new List<Vector<double>>[InputSize]);
            List<List<Vector<double>>> A = new List<List<Vector<double>>>(new List<Vector<double>>[InputSize]);

            double OldError, NewError;

            TotalError = EPS + 1d;

            StreamWriter file = new StreamWriter("Errors.csv");
            while (TotalError > EPS)
            {
                OldError = CostFunction(_input, _output);

                for (int x = 0; x < InputSize; x++)
                {
                    List<Vector<double>> Zx, Ax, Dx;
                    Zx = new List<Vector<double>>(new Vector<double>[L + 1]);
                    Ax = new List<Vector<double>>(new Vector<double>[L + 1]);
                    Dx = new List<Vector<double>>(new Vector<double>[L + 1]);

                    //Set input layer
                    Ax[1] = _input[x];

                    //Compute Zx and Ax
                    for (int l = 2; l <= L; l++)
                    {
                        Zx[l] = W[l] * Ax[l - 1] + B[l];
                        Ax[l] = Zx[l].Map(y => SigmoidFunction(y), Zeros.Include);
                    }

                    //Compute Dx with backpropagation
                    //Compute DLx
                    Dx[L] = (Ax[L] - _output[x]).PointwiseMultiply(Zx[L].Map(y => dSigmoidFunction(y), Zeros.Include));

                    //Backpropagate the error
                    for (int l = L - 1; l >= 2; l--)
                    {
                        Matrix<double> WTM = W[l + 1].Transpose();
                        Vector<double> DxM = Dx[l + 1];

                        Vector<double> WTMV = WTM * DxM;
                        Vector<double> ZxldS = Zx[l].Map(y => dSigmoidFunction(y), Zeros.Include);

                        Vector<double> _Dx = Vector<double>.Build.Dense(WTMV.Count);
                        for(int u = 0; u < _Dx.Count; u++)
                            _Dx[u] = WTMV[u] * ZxldS[u];

                        Dx[l] = ((W[l + 1].Transpose()) * Dx[l + 1]).PointwiseMultiply(Zx[l].Map(y => dSigmoidFunction(y), Zeros.Include));
                    }

                    //Add Ax and Dx to their vectors
                    A[x] = Ax;
                    D[x] = Dx;

                }

                List<Matrix<double>> AuxW = W;
                List<Vector<double>> AuxB = B;

                //Gradient descent
                for (int l = L; l >= 2; l--)
                {
                    Matrix<double> dW = Matrix<double>.Build.Dense(W[l].RowCount, W[l].ColumnCount, 0d);
                    for (int x = 0; x < InputSize; x++)
                    {
                        Matrix<double> aux1 = Matrix<double>.Build.Dense(D[x][l].Count, 1);
                        for (int pos = 0; pos < D[x][l].Count; pos++)
                            aux1[pos, 0] = D[x][l][pos];

                        Matrix<double> aux2 = Matrix<double>.Build.Dense(1, A[x][l - 1].Count);
                        for (int pos = 0; pos < A[x][l - 1].Count; pos++)
                            aux2[0, pos] = A[x][l - 1][pos];

                        Matrix<double> _dW = aux1 * aux2;
                        dW += _dW;
                    }

                    Vector<double> dB = Vector<double>.Build.Dense(D[0][l].Count, 0d);
                    for (int x = 0; x < InputSize; x++)
                        dB += D[x][l];

                    //Update Weights
                    for (int i = 0; i < W.Count; i++)
                    {
                        //Gradient descent with momentum
                        vW[l] = (momentum * vW[l]) - (learningRate * dW);
                        AuxW[l] = W[l] + vW[l];

                        //Simple Gradient Descent
                        //AuxW[l] = W[l] - ((learningRate / InputSize) * dW);
                    }

                    //Update Biases
                    vB[l] = (momentum * vB[l]) - (learningRate * dB);
                    AuxB[l] = B[l] + vB[l];

                    //Simple Gradient Descent
                    //AuxB[l] = B[l] - ((learningRate / InputSize) * dB);
                }

                W = AuxW;
                B = AuxB;

                NewError = CostFunction(_input, _output);
                TotalError = NewError;

                file.WriteLine(TotalError);
                Console.WriteLine(TotalError);
            }

            file.Close();
        }

        public List<double> Predict(List<double> input)
        {
            List<Vector<double>> Z, A;
            Z = new List<Vector<double>>(new Vector<double>[L + 1]);
            A = new List<Vector<double>>(new Vector<double>[L + 1]);

            //Set input layer
            A[1] = Vector<double>.Build.DenseOfEnumerable(input);
            for (int l = 2; l <= L; l++)
            {
                Z[l] = W[l] * A[l - 1] + B[l];
                A[l] = Z[l].Map(x => SigmoidFunction(x), Zeros.Include);
            }

            List<double> result = new List<double>(A[L].Enumerate());
            return result;
        }

        private Vector<double> privatePredict(Vector<double> input)
        {
            List<Vector<double>> Z, A;
            Z = new List<Vector<double>>(new Vector<double>[L + 1]);
            A = new List<Vector<double>>(new Vector<double>[L + 1]);

            //Set input layer
            A[1] = input;
            for (int l = 2; l <= L; l++)
            {
                Z[l] = W[l] * A[l - 1] + B[l];
                A[l] = Z[l].Map(x => SigmoidFunction(x), Zeros.Include);
            }

            return A[L];
        }

        private double CostFunction(Vector<double> input, Vector<double> desiredOutput)
        {
            Vector<double> ActualOutput = privatePredict(input);
            Vector<double> Result = (desiredOutput - ActualOutput).Map(x => x * x) / 2d;
            double Cost = 0d;

            for (int i = 0; i < Result.Count; i++)
                Cost += Result[i];

            return Cost;
        }

        private double CostFunction(List<Vector<double>> input, List<Vector<double>> desiredOutput)
        {
            double InputSize = input.Count;
            double OutputSize = desiredOutput.Count;
            double Cost = 0d;

            //Quadratic cost
            Vector<double> TotalResult = Vector<double>.Build.Dense(desiredOutput[0].Count, 0d);
            for (int j = 0; j < InputSize; j++)
            {
                Vector<double> ActualOutput = privatePredict(input[j]);
                Vector<double> Result = (desiredOutput[j] - ActualOutput).Map(x => x * x);
                TotalResult = TotalResult + Result;
            }
            for (int i = 0; i < TotalResult.Count; i++)
                Cost += TotalResult[i];

            return Cost;
        }

        public double SigmoidFunction(double x)
        {
            return 1d / (1d + Math.Exp(-x));
        }

        public double dSigmoidFunction(double x)
        {
            double y = SigmoidFunction(x);
            return (1d - y) * y;
        }

    }
}
