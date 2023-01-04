using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Neurons
{
    public class HiddenNeuron : Neuron, IComparable
    {
        private Synapse[] connections;
        private double error;

        internal static Func<double, double> activationFunction;

        public HiddenNeuron(double value)
        {
            this.value = value;
        }

        public HiddenNeuron(Synapse[] connections)
        {
            this.connections = connections;
        }

        /// <summary>
        /// Fires the neuron, calculating its value based on the previous layer.
        /// </summary>
        public void Fire()
        {
            double total = 0.0;

            for (int i = 0; i < connections.Length; i++)
            {
                total += connections[i].Value;
            }

            //Set the final value of the neuron.
            value = activationFunction(total);
        }

        public int CompareTo(object obj)
        {
            #warning Error handling would be good here.
            HiddenNeuron hiddenNeuron = (HiddenNeuron)obj;

            if(hiddenNeuron == this)
            {
                return 0;
            }
            else
            {
                return value.CompareTo(hiddenNeuron.value);
            }
        }

        internal Synapse[] Connections
        {
            get
            {
                return connections;
            }
        }

        public void AdjustForError()
        {
            foreach(Synapse synapse in connections)
            {
                //The new weights value is calculated as weight + learning rate * error * input.
                synapse.Weight = synapse.Weight + NeuralNetwork.learningRate * error * synapse.Origin.Value;
            }
        }

        public double Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }

        }

        public int SynapseCount
        {
            get
            {
                return connections.Length;
            }
        }
    }
}
