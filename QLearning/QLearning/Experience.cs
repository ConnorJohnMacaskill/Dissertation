using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QLearning
{
#warning make this internal again later.
    [Serializable()]
    public class Experience<S> : ISerializable
    {
        private S[] state;
        private int action;
        private double reward;
        private double qValue;

        public Experience(S[] state, int action, double qValue)
        {
            this.state = state;
            this.action = action;
            this.qValue = qValue;
        }

        /// <summary>
        /// Serialisation constructor, as required by ISerializable.
        /// </summary>
        public Experience(SerializationInfo info, StreamingContext ctxt)
        {
            state = (S[])info.GetValue("State", typeof(S[]));
            action = (int)info.GetValue("Action", typeof(int));
            reward = (int)info.GetValue("Reward", typeof(int));
            qValue = (double)info.GetValue("QValue", typeof(double));
        }

        #region Public Properties

        public S[] State
        {
            get
            {
                return state;
            }
        }

        public int Action
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
            }
        }

        public double Reward
        {
            get
            {
                return reward;
            }
            set
            {
                reward = value;
            }
        }

        public double QValue
        {
            get
            {
                return qValue;
            }
            set
            {
                qValue = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Used by the serializer to serialize the file.
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("State", state);
            info.AddValue("Action", action);
            info.AddValue("Reward", reward);
            info.AddValue("QValue", qValue);
        }

        #endregion Public Methods
    }
}
