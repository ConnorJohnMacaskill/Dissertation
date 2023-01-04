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
        private S[] previousState;
        private int previousAction;

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

        public int ChooseAction(S[] state, List<int> possibleActions, bool maximiser)
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
                //Pick the best experience based on Q values, if no actions exist in memory then just pick a random action.
                //Get the experiences for this state.
                List<Experience<S>> experiences = memory.GetExperiencesForState(state, possibleActions);

                if (maximiser)
                {
                    //Get the experiences with the highest Q value.
                    experiences = experiences.Where(x => x.QValue == experiences.Max(m => m.QValue)).ToList();
                }
                else
                {
                    //Get the experiences with the lowest Q value.
                    experiences = experiences.Where(x => x.QValue == experiences.Min(m => m.QValue)).ToList();
                }

                //Pick an action at random from the most ideal actions.
                action = experiences[Utility.RandomInteger(0, experiences.Count)].Action;
            }

            previousState = state;
            previousAction = action;
            return action;
        }

        public void Learn(S[] state, S[] resultState, int action, double reward, List<int> possibleActions, bool maximiser, bool terminalState)
        {
            Experience<S> experience = memory.GetExperience(state, action);

            //Train the experience.
            experience.Reward = reward;
            double expectedQ = 0.0;


            if (!terminalState)
            {
                List<Experience<S>> qValues = memory.GetExperiencesForState(resultState, possibleActions);
#warning Simplify this later.
                if (maximiser)
                {
                    expectedQ = reward + (gamma * qValues.Min(x => x.QValue));
                    //Console.WriteLine(string.Format("Training maximiser learner. Q : {0} , Reward : {1}", expectedQ, reward));
                }
                else
                {
                    expectedQ = reward + (gamma * qValues.Max(x => x.QValue));
                    //Console.WriteLine(string.Format("Training minimiser learner. Q : {0} , Reward : {1}", expectedQ, reward));
                }
            }
            else
            {
                //Q value is the reward for terminal states.
                expectedQ = reward;

                //Console.WriteLine(string.Format("Training terminal learner. Q : {0} , Reward : {1}", expectedQ, reward));
            }

            double change = alpha * (expectedQ - experience.QValue);
            experience.QValue += change;
        }

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
