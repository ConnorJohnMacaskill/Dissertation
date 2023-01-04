using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtificialNeuralNetwork;

namespace DeepQLearning
{
    public class DeepLearner
    {
        private NeuralNetwork neuralNetwork;
        private Replay<double> actionReplay;
        private double[] state;
        private int action;

        //Chance of random exploration.
        private double epsilon = 0.3;
        //Learning rate.
        private double alpha = 0.3;
        //Discount factor.
        private double gamma = 0.9;


        public DeepLearner(int stateSize)
        {
            int inputNeurons = stateSize + 1;
            int hiddenNeurons = (stateSize / 2) + 2;
            neuralNetwork = NetworkBuilder.CreateNeuralNetwork(inputNeurons, 1, stateSize, hiddenNeurons, 0.1, SigmoidFunction, SigmoidDerivative);
            actionReplay = new Replay<double>();
        }

        public int GetAction(double[] state, List<int> possibleActions)
        {
            this.state = state;

            if (possibleActions.Count < 1)
            {
                //Throw an exception here?
                return -1;
            }

            //Exploration vs exploitation.
            if (Utility.RandomDouble() < epsilon)
            {
                //Exploration!
                action = possibleActions[Utility.GetRandomInt(0, possibleActions.Count)];
            }
            else
            {
                double[] qValues = GetQValuesForState(state, possibleActions);

                //Get the index of the largest q value.
                int index = qValues.ToList().IndexOf(qValues.Max(x => x));

                //Best action is the action with the highest q value.
                action = possibleActions[index];
            }

            return action;
        }

        double[] GetQValuesForState(double[] state, List<int> possibleActions)
        {
            //Exploitation!
            double[] qValues = new double[possibleActions.Count];
            List<double> input = state.ToList();


            //Ge the approximate Q value of each action using the neural network.
            for (int i = 0; i < possibleActions.Count; i++)
            {
                int action = possibleActions[i];

                //Add action to the input.
                input.Add(action);

                neuralNetwork.SetInput(input.ToArray());
                neuralNetwork.Run();
                //Appromiate q value is the first index of the output array.
                qValues[i] = neuralNetwork.Output()[0];

                //Remove action from input to prepare for next iteration.
                input.Remove(input.Last());
            }

            return qValues;
        }

        /// <summary>
        /// Add the newest experience to the learner and give the learner an oppurtunity to learn.
        /// </summary>
        /// <param name="newState"></param>
        /// <param name="reward"></param>
        public void Train(double[] newState, double reward, List<int> possibleActionsForNewState)
        {
            //Create an experrience for our new information and add it to the replay memory.
            Experience experience = new Experience(state, newState, action, reward, possibleActionsForNewState);
            actionReplay.AddExperience(experience);

            //Get a sample of experiences from the replay.
            List<Experience> sample = actionReplay.GetSample();

            //Use the sample to train the neural network.
            for(int i = 0; i < sample.Count; i++)
            {
                double[] input = sample[i].GetFullState();
                neuralNetwork.SetInput(input);
                neuralNetwork.Run();

                //Get the desired output and use it to train the network.
                double desiredOutput = DesiredOutput(sample[i]);
                //Even though we only have one output the neural network still expects an array.
                double[] outputArray = { desiredOutput };
                neuralNetwork.Train(outputArray);
            }
        }

        private double DesiredOutput(Experience experience)
        {
            //If the experience is a terminal state then the Q value is the reward.
            if(experience.NewState == null)
            {
                return experience.Reward;
            }

            //Else the expected Q value is the reward + the discount factor multipled by the maximum q value of the next state.
            double[] qValues = GetQValuesForState(experience.NewState, experience.ActionsforNewState);
            //Get the highest Q value.
            double bestQValue = qValues.Max(x => x);

            return (experience.Reward + gamma * bestQValue);
        }

        #region Activation & Derivative functions.

        private double SigmoidFunction(double value)
        {
            return 1 / (1 + Math.Exp(-value));
        }

        private double TanHFunction(double value)
        {
            return Math.Tanh(value);
        }

        private double SigmoidDerivative(double value)
        {
            return value * (1 - value);
        }

        private double TanHDerivative(double value)
        {
            return 1 - Math.Pow(value, 2);
        }

        #endregion
    }
}
