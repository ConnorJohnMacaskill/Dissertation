using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm;
using ArtificialNeuralNetwork;
using NaughtsAndCrosses;

namespace GeneticTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Tester tester = new Tester(10);
            tester.RunTests();
        }
    }
}
