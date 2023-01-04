using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearning;

namespace QTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Learner<int> learner = new Learner<int>();

            //As a test, give it a reward for guessing 2 out of 1,2,3 for state [2,3]

            int[] state = { 2, 3 };
            int[] newState1 = { 3, 4 };
            int[] newState2 = { 4, 5 };
            int[] newState3 = { 5, 6 };
            List<int> actions = new List<int>();
            actions.Add(1);
            actions.Add(2);
            actions.Add(3);

            while (true)
            {

                int action = learner.ChooseAction(state, actions);

                if (action == 2)
                {
                    learner.Learn(newState2, 50);
                }

                if (action == 1)
                {
                    learner.Learn(newState1, 49);
                }

                if (action == 3)
                {
                    learner.Learn(newState3, 20);
                }
            }

        }
    }
}
