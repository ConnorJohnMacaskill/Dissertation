using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepQLearning;

namespace DeepQTest
{
    class Tester
    {
        DeepLearner deepLearner;

        public Tester()
        {
            deepLearner = new DeepLearner(6);
        }

        public void Test()
        {
            List<double[]> states =  new List<double[]>();
            double[] state1 = { 1, 0, 0, 0, 0, 0 };
            double[] state2 = { 0, 1, 0, 0, 0, 0 };
            double[] state3 = { 0, 0, 1, 0, 0, 0 };
            double[] state4 = { 0, 0, 0, 1, 0, 0 };
            double[] state5 = { 0, 0, 0, 0, 1, 0 };
            double[] state6 = { 0, 0, 0, 0, 0, 1 };
            //double[] state3 = { 0, 0, 1, -1, 0, 0 };

            states.Add(state1);
            states.Add(state2);
            states.Add(state3);
            states.Add(state4);
            states.Add(state5);
            states.Add(state6);

            List<int> possibleActions = new List<int>();
            
            for(int i = 0; i < state1.Length; i++)
            {
                possibleActions.Add(i);
            }

            double totalReward = 0;
            int counter = 0;

            while (true)
            {


                for (int i = 0; i < states.Count; i++)
                {
                    int action = deepLearner.GetAction(states[i], possibleActions);

                    double reward = states[i][action];

                    counter += 1;
                    totalReward += reward;
                    deepLearner.Train(null, reward, null);

                    
                }

                Console.WriteLine("Average = " + totalReward / (double)counter);
            }

        }
    }
}
