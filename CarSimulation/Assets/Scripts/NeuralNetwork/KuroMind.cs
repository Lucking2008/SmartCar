using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuroMind
{
    public class KuroMind
    {
        //memory
        public Double[, ,] w { get; set; }
        public Double[, ,] p { get; set; }
        public Double[,] o { get; set; }
        public Double[,] e { get; set; }
        public Int32[] l { get; set; }


        //learning parameters
        public Double alpha { get; set; }
        public Double beta { get; set; }

        public void Initialize(List<Int32> layerSize,Double alpha=0.3,Double beta = 0.4)
        {
            this.alpha = alpha; this.beta = beta;
            this.l = layerSize.ToArray();
            w = new Double[layerSize.Count, layerSize.Max() + 1, layerSize.Max() + 1];
            p = new Double[layerSize.Count, layerSize.Max() + 1, layerSize.Max() + 1];
            Random r = new Random();

            for (int i = 0; i < layerSize.Count - 1; i++)
                for (int k = 0; k < l[i] + 1; k++)
                    for (int m = 0; m < l[i + 1]; m++)
                    {
                        w[i, k, m] = (r.NextDouble()-0.5)*2;
                        p[i, k, m] = 0;
                    }
            o = new Double[l.Count(), l.Max() + 1];
            e = new Double[l.Count(), l.Max() + 1];
        }

        public Double sigmoid(Double t)
        {
            return 1 / (1 + Math.Pow(Math.E, -1 * t));
        }
        public void Predict(List<Double> x)
        {
            for (int i = 0; i < l.Count(); i++)
                for (int k = 0; k < l[i] + 1; k++)
                    o[i, k] = l[i] == k ? 1 : 0;

            for (int i = 0; i < x.Count; i++)
                o[0, i] = x[i];

            for (int i = 0; i < l.Count() - 1; i++)
            {
                for (int k = 0; k < l[i] + 1; k++)
                    for (int m = 0; m < l[i + 1]; m++)
                        o[i + 1, m] += w[i, k, m] * o[i, k];

                for (int k = 0; k < l[i+1]; k++)
                    o[i+1, k] = sigmoid(o[i+1, k]);
            }


        }
        public void LearnOneExample(List<Double> x, List<Double> y)
        {
            for (int i = 0; i < l.Count(); i++)
                for (int k = 0; k < l[i] + 1; k++)
                    e[i, k] = 0;
            Predict(x);


            //get error for output layer
            int s = l.Count() - 1;
            for (int i = 0; i < l[s]; i++)
                e[s, i] =o[s,i] *(1 - o[s, i]) * (y[i] - o[s, i]);


            // back prop the error
            for (int i = l.Count() - 2; i > 0; i--)
            {

                for (int k = 0; k < l[i] + 1; k++)
                    for (int m = 0; m < l[i + 1]; m++)
                        e[i, k] += w[i, k, m] * e[i+1,m];

                for (int k = 0; k < l[i] + 1; k++)
                {
                    e[i, k] = o[i, k] * (1 - o[i, k]) * e[i, k];
                }
            }
            //Apply momentum
            for (int i = 0; i < l.Count() - 1; i++)
                for (int k = 0; k < l[i] + 1; k++)
                    for (int m = 0; m < l[i + 1]; m++)
                    {
                        w[i, k, m] += alpha * p[i, k, m];
                       
                    }

            //Adjust weights
            for (int i = 0; i < l.Count() -1; i++)
                for (int k = 0; k < l[i] + 1; k++)
                    for (int m = 0; m < l[i + 1]; m++)
                    {
                        p[i, k, m] = beta * e[i+1, m] * o[i, k];
                        w[i, k, m] += p[i, k, m];
                    }
        }

        public Int32 LearnVectorOfExample(List<List<Double>> lstX,List<List<Double>> lstY,double maxError=0.01)
        {
            Double error = 1;
            Int32 it = 0;
            while(error>=maxError)
            {
                error = 0;
                it++;
                for (int i = 0; i < lstY.Count; i++)
                {
                    LearnOneExample(lstX[i], lstY[i]);
                    error += SquareError(lstY[i]);

                }
                error /= lstY.Count();
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
            Int32 s = l.Count()-1;
            for (int i = 0; i < y.Count; i++)
            {
                sum += (o[s, i] - y[i])*(o[s, i] - y[i]) /2;
            }
            return sum;
        }
        public void printWeights()
        {
            for (int i = 0; i < l.Count() - 1; i++)
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
            int s = l.Count() - 1;
            for (int i = 0; i < l[s]; i++)
            {
                Console.Write(" " + o[s, i]);
            }
            Console.WriteLine("");
        }
    }
}
