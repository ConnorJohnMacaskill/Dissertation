using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialNeuralNetwork.Exceptions
{
    class NetworkInputInvalidException : Exception
    {
        private int inputCount;
        private int inputNeuronCount;
        private const string EXCEPTION_MESSAGE = "Number of inputs does not match number of input neurons!";

        public NetworkInputInvalidException(int inputCount, int inputNeuronCount) : base(EXCEPTION_MESSAGE)
        {
            this.inputCount = inputCount;
            this.inputNeuronCount = inputNeuronCount;
        }

        public int InputCount
        {
            get
            {
                return inputCount;
            }
        }

        public int InputNeuronCount
        {
            get
            {
                return inputNeuronCount;
            }
        }
    }
}
