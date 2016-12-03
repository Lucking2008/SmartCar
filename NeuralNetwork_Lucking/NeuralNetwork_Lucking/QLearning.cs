using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork_Lucking
{
    class QLearning
    {
        public struct state {
            public int x;
            public int y;
        }

        public enum action {
            up,
            down,
            left,
            right
        };

        public double[] actionTable = {
            0.0,
            0.25,
            0.50,
            0.75
        };

        int epochs = 5000;
        double epsilon = 1d;

        NeuralNetwork Qfunction;

        state actualState;

        public QLearning()
        {
            Qfunction = new NeuralNetwork();
            Qfunction.Initialize(new List<int> { 2, 3, 3, 4 });

            Qfunction.learningRate = 0.1;

            actualState = new state();
            actualState.x = 0;
            actualState.y = 0;

        }

        public void ResetState()
        {
            actualState.x = 0;
            actualState.y = 0;
        }

        public void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(actualState.x, actualState.y);
            Console.Write("*");
        }

        public void Move()
        {
            List<double> qval = Qfunction.Predict(new List<double> { actualState.x / 80d, actualState.y / 30d });

            double best = qval[0];
            int pos = 0;
            for (int i = 0; i < 4; i++)
                if (qval[i] > best)
                {
                    best = qval[i];
                    pos = i;
                }

            double a = actionTable[pos];

            state nextState = new state();
            nextState.x = actualState.x;
            nextState.y = actualState.y;
            switch ((int) (a * 4d))
            {
                case (int)action.up: nextState.y--; break;
                case (int)action.down: nextState.y++; break;
                case (int)action.left: nextState.x--; break;
                case (int)action.right: nextState.x++; break;
            }

            actualState = new state();
            actualState.x = nextState.x;
            actualState.y = nextState.y;

            if (nextState.x < 0 || nextState.x > 80 || nextState.y < 0 || nextState.y > 30)
            {
                ResetState();
            }

        }

        public void Update()
        {
            List<double> qval = Qfunction.Predict(new List<double> { actualState.x / 80d, actualState.y / 30d });

            double best = qval[0];
            int pos = 0;
            for (int i = 0; i < 4; i++)
                if (qval[i] > best)
                {
                    best = qval[i];
                    pos = i;
                }

            double a;

            Random r = new Random();
            if (r.NextDouble() < epsilon)
            {
                a = actionTable[r.Next(4)];
            }        
            else
                a = actionTable[pos];

            state nextState = new state();
            nextState.x = actualState.x;
            nextState.y = actualState.y;
            switch ((int)(a * 4d))
            {
                case (int)action.up: nextState.y--; break;
                case (int)action.down: nextState.y++; break;
                case (int)action.left: nextState.x--; break;
                case (int)action.right: nextState.x++; break;
            }
            double newReward = getReward(actualState, nextState);

            List<double> newQ = Qfunction.Predict(new List<double> { nextState.x, nextState.y });
            double maxQ = newQ.Max();

            double gamma = 0.5;
            double update = Qfunction.SigmoidFunction(qval[pos] + Qfunction.learningRate * (newReward + gamma * maxQ - qval[pos]));

            qval[pos] = update;

            List<double> input = new List<double> { actualState.x, actualState.y };
            List<double> output = qval;

            Qfunction.LearnIteration(new List<List<double>> { input }, new List<List<double>> { output });

            actualState = new state();
            actualState.x = nextState.x;
            actualState.y = nextState.y;

            if (nextState.x < 0 || nextState.x > 80 || nextState.y < 0 || nextState.y > 30)
            {
                ResetState();              
            }

            if (epsilon > 0.1)
                epsilon -= (1d / epochs);

        }

        public double getReward(state actualState, state nextState)
        {
            int distX = nextState.x - actualState.x;
            int distY = nextState.y - actualState.y;
            if (distX == -1 && distY == -1)
                return -50;
            else
                return distX * 30 + distY * 10;

        }
    }
}
