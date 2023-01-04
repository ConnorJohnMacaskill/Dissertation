using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace QLearning
{
    public class Learner<S>
    {
        //Chance of random exploration.
        private double epsilon = 0.3;
        //Learning rate.
        private double alpha = 0.3;
        //Discount factor.
        private double gamma = 0.9;
        public Memory<S> memory;
        private Tuple<S[], int> previousAction;

        public Learner(double epsilon = 0.3, double alpha = 0.3, double gamma = 0.9)
        {
            this.epsilon = epsilon;
            this.alpha = alpha;
            this.gamma = gamma;
            memory = new Memory<S>();
        }

        public Learner(string filePath, double epsilon = 0.3, double alpha = 0.3, double gamma = 0.9)
        {
            this.epsilon = epsilon;
            this.alpha = alpha;
            this.gamma = gamma;

            if(!Load(filePath))
            {
                //File doesnt exist, instantiate a new memory object.
                memory = new Memory<S>();
            }
        }

        /// <summary>
        /// Returns what the learner considers the best action for the state.
        /// </summary>
        public int ChooseAction(S[] state, List<int> possibleActions)
        {
            int action;

            if (possibleActions.Count < 1)
            {
                //Throw an exception here?
                return -1;
            }

            //Random epsilon for random exploration of other actions.
            if (Utility.RandomDouble() < epsilon)
            {
                action = possibleActions[Utility.RandomInteger(0, possibleActions.Count)];
            }
            else
            {
                //Get the experiences for this state.
                List<Experience<S>> experiences = memory.GetExperiencesForState(state, possibleActions);

                //Get the experiences with the highest Q-Value, there may be more than one with identical values.
                experiences = experiences.Where(x => x.QValue == experiences.Max(m => m.QValue)).ToList();

                //If we have more than one "best" action pick one at random. Otherwise random max is exclsuive so it will pick the best action.
                action = experiences[Utility.RandomInteger(0, experiences.Count)].Action;
            }

            previousAction = Tuple.Create(state, action);
            return action;
        }

        /// <summary>
        /// Allows the learner to learn about the new state and reward based the previously chosen action and state.
        /// </summary>
        public void Learn(S[] newState, double reward, List<int> possibleActions, bool terminalState)
        {
            //Not done anything yet.
            if (previousAction == null)
            {
                return;
            }

            Experience<S> experience = memory.GetExperience(previousAction.Item1, previousAction.Item2);

            //Train the experience.
            experience.Reward = reward;

            //If the action results in a terminal state, then the Q-Value is equal to the reward.
            if (!terminalState)
            {
                //Get Q values of every action in the new state.
                List<Experience<S>> qValues = memory.GetExperiencesForState(newState, possibleActions);

                double maxQ = 0;

                //Get the maximum Q value of the Q values in the new state.
                if (qValues.Count > 0)
                {
                    maxQ = qValues.Max(x => x.QValue);
                }

                //Calculate new QValue for this experience.
                experience.QValue = experience.QValue + alpha * ((reward + gamma * maxQ) - experience.QValue);
            }
            else
            {
                //Q value is equal to the reward in terminal states.
                experience.QValue = reward;
            }
        }

        /// <summary>
        /// Saves the learner's memory to the file path.
        /// </summary>
        public void Save(string filePath)
        {
            //Create copy of file.
            if (File.Exists(filePath))
            {
                File.Copy(filePath, "qLearnerBackup", true);
            }

            Stream stream = File.Open(filePath, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Serialize(stream, memory);
            stream.Close();
        }

        /// <summary>
        /// Loads the learner's memory from the file path, returns true if loaded successfully.
        /// </summary>
        public bool Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream stream = File.Open(filePath, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                memory = (Memory<S>)bformatter.Deserialize(stream);
                stream.Close();

                return true;
            }

            return false;
        }
    }
}
