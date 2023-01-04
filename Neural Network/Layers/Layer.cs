using ArtificialNeuralNetwork.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Layers
{
    /// <summary>
    /// Base Layer class, provides basic functionality for the Layer object.
    /// </summary>
    public abstract class Layer
    {
        protected Neuron[] neurons;

        public Neuron[] Neurons
        {
            get
            {
                return neurons;
            }
        }

        public abstract void Fire();

        public abstract int GetWeightCount();

        //Returns the values of the neurons in the layer as a nicely formatted string.
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Neuron Values : ");

            foreach (Neuron neuron in neurons)
            {
                stringBuilder.Append(string.Format("{0}, ", neuron.Value));
            }

            //Get rid of the trailing comma and space.
            stringBuilder.Remove(stringBuilder.Length - 2, 2);

            return stringBuilder.ToString();
        }
    }
}
