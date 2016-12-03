using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralNetwork_Lucking
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                NeuralNetwork nn = new NeuralNetwork();
                nn.Initialize(new List<int> { 3, 5, 5, 1 });

                List<List<double>> Inputs = new List<List<double>> {
                new List<double> { 0d, 0d, 0d },
                new List<double> { 0d, 0d, 1d },
                new List<double> { 0d, 1d, 0d },
                new List<double> { 0d, 1d, 1d },
                new List<double> { 1d, 0d, 0d },
                new List<double> { 1d, 0d, 1d },
                new List<double> { 1d, 1d, 0d },
                new List<double> { 1d, 1d, 1d }
                };
                List<List<double>> Outputs = new List<List<double>> {
                new List<double> { 0d },
                new List<double> { 1d },
                new List<double> { 1d },
                new List<double> { 0d },
                new List<double> { 1d },
                new List<double> { 0d },
                new List<double> { 0d },
                new List<double> { 0d }
                };

                nn.learningRate = 0.5d;
                nn.Learn(Inputs, Outputs);
                Console.WriteLine("Convergence reached!");

                List<List<double>> result = new List<List<double>> {
                nn.Predict(Inputs[0]),
                nn.Predict(Inputs[1]),
                nn.Predict(Inputs[2]),
                nn.Predict(Inputs[3]),
                nn.Predict(Inputs[4]),
                nn.Predict(Inputs[5]),
                nn.Predict(Inputs[6]),
                nn.Predict(Inputs[7])
                };

                Console.WriteLine("");
                foreach (List<double> ld in result)
                    foreach (double d in ld)
                        Console.WriteLine(d);

                int a = 0;
                Console.Read();
            }

            /*QLearning agent = new QLearning();
            int c = 0;
            while (true)
            {
                agent.Draw();
                if (c < 5000)
                {
                    agent.Update();
                    c++;
                }
                else
                    agent.Move();

                Thread.Sleep(10);
            }*/

        }
    }
}
