using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public static class PopulationBuilder
    {
        //Instantiates and returns a new population.
        public static Population CreatePopulation(int populationSize, GeneInfo geneInfo, Func<double[], int> fitnessFunction)
        {
            Population population = new Population(populationSize, geneInfo);
            Population.FitnessFunction = fitnessFunction;
            return population;
        }
    }
}
