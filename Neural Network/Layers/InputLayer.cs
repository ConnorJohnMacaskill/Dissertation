using ArtificialNeuralNetwork.Exceptions;
using ArtificialNeuralNetwork.Neurons;

namespace ArtificialNeuralNetwork.Layers
{
    public class InputLayer : Layer
    {
        public InputLayer(InputNeuron[] neurons)
        {
            this.neurons = neurons;
        }

        public override void Fire()
        {
            //Input layer does nothing when fired.
        }

        public override int GetWeightCount()
        {
            //The input layer has no weight connections.
            return 0;
        }

        /// <summary>
        /// Sets the values of the input neurons in this layer. Array size must equal number of input neurons.
        /// </summary>
        public void SetInput(double[] inputs)
        {
            int inputCount = inputs.Length;
            int inputNeuronCount = Neurons.Length;

            //Check we have the same number of input values and neurons.
            if (inputCount != inputNeuronCount)
            {
                throw new NetworkInputInvalidException(inputCount, inputNeuronCount);
            }
            else
            {
                for (int i = 0; i < inputCount; i++)
                {
                    neurons[i].Value = inputs[i];
                }
            }
        }
    }
}
