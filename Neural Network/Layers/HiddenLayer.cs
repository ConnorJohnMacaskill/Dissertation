using ArtificialNeuralNetwork.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Layers
{
    public class HiddenLayer : Layer
    {
        public HiddenLayer(HiddenNeuron[] neurons)
        {
            this.neurons = neurons;
        }

        /// <summary>
        /// Fire every neuron in this layer.
        /// </summary>
        public override void Fire()
        {
            foreach(HiddenNeuron neuron in neurons)
            {
                neuron.Fire();
            }
        }

        /// <summary>
        /// Returns the number of weighted connections in this hidden layer.
        /// </summary>
        public override int GetWeightCount()
        {
            int count = 0;

            foreach(HiddenNeuron neuron in neurons)
            {
                count += neuron.SynapseCount;
            }

            return count;
        }
    }
}
