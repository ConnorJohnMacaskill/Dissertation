using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Neurons
{
    public abstract class Neuron
    {
        protected double value;

        /// <summary>
        /// Returns the resulting value of the neuron after it has been fired.
        /// </summary>
        /// <returns>Neuron value as a double.</returns>
        public virtual double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
