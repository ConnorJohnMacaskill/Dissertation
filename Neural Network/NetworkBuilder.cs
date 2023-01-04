using ArtificialNeuralNetwork.Neurons;
using ArtificialNeuralNetwork.Layers;
using System;

namespace ArtificialNeuralNetwork
{
    public static class NetworkBuilder
    {
        private static Random random = new Random();

        /// <summary>
        /// Creates a new neural network with randomised weights.
        /// </summary>
        /// <param name="inputNeuronCount">Number of input neurons in the network.</param>
        /// <param name="outputNeuronCount">Number of output neurons in the network.</param>
        /// <param name="hiddenLayerCount">Number of hidden layers in the network.</param>
        /// <param name="hiddenNeuronCount">Number of hidden neurons in each hidden layer.</param>
        /// <returns></returns>
        public static NeuralNetwork CreateNeuralNetwork(int inputNeuronCount, int outputNeuronCount, int hiddenLayerCount, int hiddenNeuronCount, double learningRate, Func<double, double> activationFunction, Func<double, double> derivativeFunction)
        {
            //Set activation and derivative functions.
            HiddenNeuron.activationFunction = activationFunction;
            BackPropagation.derivitiveFunction = derivativeFunction;

            int layerTotal = 2 + hiddenLayerCount;
            Layer[] layers = new Layer[layerTotal];

            //Create a new bias neuron to act as a bias for each hidden layer.
            BiasNeuron biasNeuron = new BiasNeuron();

            //Create the input layer.
            layers[0] = GetNewInputLayer(inputNeuronCount);

            //Create the hidden layers.
            for(int i = 0; i < hiddenLayerCount; i++)
            {
                //First hidden layer is layers[1], so with i starting at 0 the previous layer is equal to layers[i].
                Layer previousLayer = layers[i];

                HiddenLayer hiddenLayer = GetNewHiddenLayer(hiddenNeuronCount, previousLayer, biasNeuron);

                //Assign new hidden layer to the array.
                layers[i + 1] = hiddenLayer;
            }

            //Output layer is essentially a mandatory hidden layer, but with a different number of neurons.
            //Output layer is the last layer aka length-1 so the previous layer is at element length-2.
            layers[layers.Length - 1] = GetNewHiddenLayer(outputNeuronCount, layers[layers.Length - 2], biasNeuron);


            return new NeuralNetwork(layers, inputNeuronCount, outputNeuronCount, hiddenLayerCount, hiddenNeuronCount, learningRate);
        }

        private static double[] GetRandomWeights(int weightCount)
        {
            double[] weights = new double[weightCount];

            //Initalise random weights.
            for (int i = 0; i < weightCount; i++)
            {
                weights[i] = random.NextDouble();
            }

            return weights;
        }

        private static InputLayer GetNewInputLayer(int inputNeuronCount)
        {
            InputNeuron[] inputNeurons = new InputNeuron[inputNeuronCount];

            for (int i = 0; i < inputNeuronCount; i++)
            {
                inputNeurons[i] = new InputNeuron();
            }

            return new InputLayer(inputNeurons);
        }

        private static HiddenLayer GetNewHiddenLayer(int hiddenNeuronCount, Layer previousLayer, BiasNeuron biasNeuron)
        {
            //Initialise random weights to connect to the previous layer.
            HiddenNeuron[] hiddenNeurons = new HiddenNeuron[hiddenNeuronCount];

            for(int i = 0; i < hiddenNeuronCount; i++)
            {
                hiddenNeurons[i] = GetHiddenNeuron(previousLayer, biasNeuron);
            }

            return new HiddenLayer(hiddenNeurons);
        }

        private static HiddenNeuron GetHiddenNeuron(Layer previousLayer, BiasNeuron biasNeuron)
        {
            //+1 to account for the bias.
            Synapse[] synapses = new Synapse[previousLayer.Neurons.Length + 1];
            double[] weights = GetRandomWeights(previousLayer.Neurons.Length + 1);

            for (int i = 0; i < synapses.Length - 1; i++)
            {
                synapses[i] = new Synapse(previousLayer.Neurons[i], weights[i]);
            }

            //Add connection to a bias neuron for the hidden layer.
            synapses[synapses.Length - 1] = new Synapse(biasNeuron, weights[weights.Length - 1]);


            return new HiddenNeuron(synapses);
        }
    }
}
