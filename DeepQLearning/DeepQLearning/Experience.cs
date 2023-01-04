using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepQLearning
{
    class Experience
    {
        private double[] state;
        private double[] newState;
        private int action;
        private double reward;
        private List<int> actionsForNewState;

        public Experience(double[] state, double[] newState, int action, double reward, List<int> actionsForNewState)
        {
            this.state = state;
            this.newState = newState;
            this.action = action;
            this.reward = reward;
            this.actionsForNewState = actionsForNewState;
        }

        public double[] State
        {
            get
            {
                return state;
            }
        }

        public double[] NewState
        {
            get
            {
                return newState;
            }
        }

        public int Action
        {
            get
            {
                return action;
            }
        }

        public double Reward
        {
            get
            {
                return reward;
            }
        }

        public List<int> ActionsforNewState
        {
            get
            {
                return actionsForNewState;
            }
        }

        public double[] GetFullState()
        {
            List<double> input = state.ToList();
            input.Add(action);

            return input.ToArray();
        }
    }
}
