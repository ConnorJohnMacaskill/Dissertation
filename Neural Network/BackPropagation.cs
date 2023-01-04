using ArtificalNeuralNetwork.Exceptions;
using ArtificialNeuralNetwork.Layers;
using ArtificialNeuralNetwork.Neurons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork
{
    internal static class BackPropagation
    {
        internal static Func<double, double> derivitiveFunction;

        internal static void Train(NeuralNetwork neuralNetwork, double[] desiredOutput)
        {
            //Ensure we have an identical number of output neurons and desired outputs.
            if (desiredOutput.Length != neuralNetwork.OutputLayer.Neurons.Length)
            {
                throw new NetworkOutputInvalidException(desiredOutput.Length, neuralNetwork.OutputLayer.Neurons.Length);
            }

            AdjustOutputLayer(neuralNetwork, desiredOutput);
            AdjustLayers(neuralNetwork);
            AdjustForError(neuralNetwork);
        }

        private static void AdjustOutputLayer(NeuralNetwork neuralNetwork, double[] desiredOutput)
        {
            for (int i = 0; i < desiredOutput.Length; i++)
            {
                HiddenNeuron outputNeuron = neuralNetwork.OutputLayer.Neurons[i] as HiddenNeuron;

                //Calculate the error delta and store it in the neuron. Then adjust for this error with the calculation weight = weight + learning rate * error delta * origin value.
                outputNeuron.Error = (desiredOutput[i] - outputNeuron.Value) * derivitiveFunction(outputNeuron.Value);
               // outputNeuron.AdjustForError();
            }
        }

        private static void AdjustLayers(NeuralNetwork neuralNetwork)
        {
            //We want to adjust all hidden layers which are between the input and output layer. We can do this by looping from the length-2 to 1.
            for(int i = neuralNetwork.Layers.Length-2; i > 0; i--)
            {
                HiddenLayer currentLayer = neuralNetwork.Layers[i] as HiddenLayer;

                foreach(HiddenNeuron neuron in currentLayer.Neurons)
                {
                    //Sum up the weighted errors.
                    double weightedErrorTotals = 0.0;

                    //For each weight from this neuron to each neuron in the next layer, sum up the weighted error of the connected neuron.
                    foreach(HiddenNeuron connectedNeuron in neuralNetwork.Layers[i+1].Neurons)
                    {
                        //Get all weights connected from this neuron to the next layer.
                        Synapse connection = connectedNeuron.Connections.First(x => x.Origin == neuron);

                        //Acumulate the weighted error total.
                        weightedErrorTotals += connection.Weight * connectedNeuron.Error;
                    }

                    //Error of the hidden neuron is the total weighted error multipled by the activation function's derivative of the neuron's value.
                    neuron.Error = weightedErrorTotals * derivitiveFunction(neuron.Value);
                    //neuron.AdjustForError();
                }
            }
        }

        private static void AdjustForError(NeuralNetwork neuralNetwork)
        {
            //Adjust the weights according to the error of every hidden neuron in the network.
            for (int i = 1; i < neuralNetwork.Layers.Length; i++)
            {
                for (int y = 0; y < neuralNetwork.Layers[i].Neurons.Length; y++)
                {
                    (neuralNetwork.Layers[i].Neurons[y] as HiddenNeuron).AdjustForError();
                }
            }
        }
    }
}
