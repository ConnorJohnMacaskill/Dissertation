using ArtificialNeuralNetwork.Layers;
using ArtificialNeuralNetwork.Neurons;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork
{
    public class NeuralNetwork
    {
        private int layerCount;
        private Layer[] layers;
        private int weightCount;

        private int inputNeuronCount;
        private int outputNeuronCount;
        private int hiddenLayerCount;
        private int hiddenNeuronCount;

        internal static double learningRate = 1.0;

        internal NeuralNetwork(Layer[] layers, int inputNeuronCount, int outputNeuronCount, int hiddenLayerCount, int hiddenNeuronCount, double learningRate = 0.2)
        {
            this.layers = layers;
            this.inputNeuronCount = inputNeuronCount;
            this.outputNeuronCount = outputNeuronCount;
            this.hiddenLayerCount = hiddenLayerCount;
            this.hiddenNeuronCount = hiddenNeuronCount;
            NeuralNetwork.learningRate = learningRate;

            layerCount = layers.Length;
            SetWeightCount();
        }

        internal Layer[] Layers
        {
            get
            {
                return layers;
            }
        }

        internal InputLayer InputLayer
        {
            get
            {
                return layers[0] as InputLayer;
            }
        }

        internal HiddenLayer OutputLayer
        {
            get
            {
                return layers[layerCount - 1] as HiddenLayer;
            }
        }

        internal int WeightCount
        {
            get
            {
                return weightCount;
            }
        }

        public int HiddenLayerCount
        {
            get
            {
                return hiddenLayerCount;
            }
        }

        public double LearningRate
        {
            get
            {
                return learningRate;
            }
        }

        /// <summary>
        /// Returns a nicely formatted string of the network's input.
        /// </summary>
        public string InputToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Network Inputs");

            for (int i = 0; i < InputLayer.Neurons.Length; i++)
            {
                stringBuilder.AppendLine(string.Format("{0} : {1}", i, InputLayer.Neurons[i].Value));
            }

            stringBuilder.AppendLine("");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns a nicely formatted string of the network's output.
        /// </summary>
        /// <returns></returns>
        public string OutputToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Network Outputs");
            
            for(int i = 0; i < OutputLayer.Neurons.Length; i++)
            {
                stringBuilder.AppendLine(string.Format("{0} : {1}", i, OutputLayer.Neurons[i].Value));
            }

            stringBuilder.AppendLine("");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Trains the network by adjusting its weights in an effort to move the actual output closer to the desired output.
        /// </summary>
        public void Train(double[] desiredOutputs)
        {
            BackPropagation.Train(this, desiredOutputs);
        }

        /// <summary>
        /// Execute a feed forward firing of the network.
        /// </summary>
        public void Run()
        {
            foreach(Layer layer in layers)
            {
                layer.Fire();
            }
        }

        /// <summary>
        /// Sets the network's input.
        /// </summary>
        /// <param name="inputs">Network input, must be equal in size to the number of input neurons.</param>
        public void SetInput(double[] inputs)
        {
            InputLayer layer = layers[0] as InputLayer;

            layer.SetInput(inputs);
        }

        /// <summary>
        /// Sets the weights of the network.
        /// </summary>
        /// <param name="weights">Every weight as a double array.</param>
        private void SetWeights(double[] weights)
        {
            if(weights.Length != weightCount)
            {
                #warning Could probably put better exception handling here.
                throw new Exception("Weight length does not match weight count!");
            }

            int counter = 0;

            for (int i = 1; i < layers.Length; i++)
            {
                HiddenLayer layer = (HiddenLayer)layers[i];

                foreach(HiddenNeuron neuron in layer.Neurons)
                {
                    foreach(Synapse synapse in neuron.Connections)
                    {
                        synapse.Weight = weights[counter];
                        counter += 1;
                    }
                }           
            }
        }

        /// <summary>
        /// Returns the network's output as a double array.
        /// </summary>
        public double[] Output()
        {
            double[] results = new double[OutputLayer.Neurons.Length];
            for(int i = 0; i < results.Length; i++)
            {
                results[i] = OutputLayer.Neurons[i].Value;
            }
            return results;
        }

        /// <summary>
        /// Saves the network's weights to the desired filepath.
        /// </summary>
        public void Save(string filePath)
        {
            double[] weights = new double[weightCount];

            int index = 0;

            for (int i = 1; i < layers.Length; i++)
            {
                HiddenLayer layer = (HiddenLayer)layers[i];

                foreach (HiddenNeuron neuron in layer.Neurons)
                {
                    foreach (Synapse synapse in neuron.Connections)
                    {
                        weights[index] = synapse.Weight;
                        index += 1;
                    }
                }
            }


            File.WriteAllLines(filePath, weights.Select(d => d.ToString()).ToArray());
        }

        /// <summary>
        /// Load the network's weights from the filepath.
        /// </summary>
        /// <param name="filePath"></param>
        public void Load(string filePath)
        {
            string[] stringWeights = File.ReadAllLines(filePath);

            SetWeights(Array.ConvertAll(stringWeights, x => double.Parse(x)));
        }

        //Calculates the number of weighted connections in the network.
        private void SetWeightCount()
        {
            weightCount = 0;

            foreach(Layer layer in layers)
            {
                weightCount += layer.GetWeightCount();
            }
        }
    }
}
