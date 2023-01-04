using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace QLearning
{
    [Serializable()]
    internal class Memory<S> : ISerializable
    {
        public List<Experience<S>> experiences;

        public Memory()
        {
            this.experiences = new List<Experience<S>>();
        }

        public Memory(SerializationInfo info, StreamingContext ctxt)
        {
            experiences = (List<Experience<S>>)info.GetValue("Experiences", typeof(List<Experience<S>>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Experiences", experiences);
        }

        /// <summary>
        /// Returns the relevant experience for the state action pair, adds it if it has not yet been encountered.
        /// </summary>
        internal Experience<S> GetExperience(S[] state, int action)
        {
            Experience<S> experience = null;

            //Filter the list down to the relevant experience, if it exists.
            List<Experience<S>> stateExperiences = experiences.Where(x => Enumerable.SequenceEqual(x.State, state) && x.Action == action).ToList();

            if (stateExperiences.Count == 0)
            {
                //Start Q value higher than 0 to encourage exploration.
                experience = new Experience<S>(state, action, 1.0);
                experiences.Add(experience);
            }
            else
            {
                experience = stateExperiences[0];
            }

            return experience;
        }

        /// <summary>
        /// Returns all experiences related to the state and the possible actions.
        /// </summary>
        internal List<Experience<S>> GetExperiencesForState(S[] state, List<int> possibleActions)
        {
            //Add any experiences we have not yet had for each possible state action pair.
            possibleActions.ForEach(x => GetExperience(state, x));

            //Return all experiences we have for this state.
            return experiences.Where(x => Enumerable.SequenceEqual(x.State, state)).ToList();
        }
    }
}
