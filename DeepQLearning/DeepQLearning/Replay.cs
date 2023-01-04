using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepQLearning
{
    class Replay<T>
    {
        //Might need to make this a tad smaller.
        private const int MAX_MEMORY_SIZE = int.MaxValue;
        private const int SAMPLE_SIZE = 30;
        private const int LEARN_THRESHOLD = 30;

        List<Experience> replayMemory;

        public Replay()
        {
            replayMemory = new List<Experience>();
        }

        public List<Experience> GetSample()
        {
            List<Experience> sample = new List<Experience>();

            //We don't want to take a sample until we have a good amount to sample from.
            if(replayMemory.Count >= LEARN_THRESHOLD)
            {
                for (int i = 0; i < SAMPLE_SIZE; i++)
                {
                    int index = Utility.GetRandomInt(0, replayMemory.Count);

                    if (!sample.Contains(replayMemory[i]))
                    {
                        sample.Add(replayMemory[i]);
                    }
                    else
                    {
                        i -= 1;
                    }
                }
            }

            return sample;
        }

        public void AddExperience(Experience experience)
        {
            //Remove old experiences for this state/action pair.
            Experience oldExperience = replayMemory.Where(x => x.State == experience.State && x.Action == experience.Action).FirstOrDefault();

            if(oldExperience != null)
            {
                replayMemory.Remove(oldExperience);
            }

            //If the list exceeds our limit, remove the oldest experience and add the new one.
            if(replayMemory.Count >= MAX_MEMORY_SIZE)
            {
                replayMemory.RemoveAt(0);
            }

            replayMemory.Add(experience);
        }
    }
}
