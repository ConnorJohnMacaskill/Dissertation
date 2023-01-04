using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Neurons
{
    public class InputNeuron : Neuron
    {
        public InputNeuron()
        {
            value = 1.0;
        }

        public void SetValue(double value)
        {
            this.value = value;
        }

    }
}
