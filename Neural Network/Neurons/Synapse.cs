using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Neurons
{
    public class Synapse
    {
        private Neuron origin;
        private double weight;

        public Synapse(Neuron origin)
        {
            this.origin = origin;
            this.weight = 1.0;
        }

        public Synapse(Neuron origin, double weight)
        {
            this.origin = origin;
            this.weight = weight;
        }

        public Neuron Origin
        {
            get
            {
                return origin;
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        public double Value
        {
            get
            {
                return (origin.Value * weight);
            }
        }
    }
}
