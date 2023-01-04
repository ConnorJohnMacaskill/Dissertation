using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificalNeuralNetwork.Exceptions
{
    class NetworkOutputInvalidException : Exception
    {
        private int outputCount;
        private int outputNeuronCount;
        private const string EXCEPTION_MESSAGE = "Number of expected outputs does not match number of output neurons!";

        public NetworkOutputInvalidException(int outputCount, int outputNeuronCount) : base(EXCEPTION_MESSAGE)
        {
            this.outputCount = outputCount;
            this.outputNeuronCount = outputNeuronCount;
        }

        public int OutputCount
        {
            get
            {
                return outputCount;
            }
        }

        public int OutputNeuronCount
        {
            get
            {
                return outputNeuronCount;
            }
        }
    }
}