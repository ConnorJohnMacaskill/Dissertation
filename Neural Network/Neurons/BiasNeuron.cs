using ArtificialNeuralNetwork.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Neurons
{
    class BiasNeuron : Neuron
    {
        public BiasNeuron()
        {
            //Bias neurons have a constant value of 1.
            value = 1;
        }

        //We don't want the bias neuron to ever change value.
        public override double Value
        {
            get
            {
                return value;
            }
        }
    }
}
