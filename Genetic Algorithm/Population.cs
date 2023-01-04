using System;

namespace GeneticAlgorithm
{
    public class Population
    {
        private Individual[] individuals;
        private static Func<double[], int> fitnessFunction;

        public Population(int populationSize, GeneInfo geneInfo)
        {
            individuals = new Individual[populationSize];
            
            GeneratePopulation(geneInfo);
        }

        public Population(Individual[] individuals)
        {
            this.individuals = individuals;
        }

        #region Public Properties

        public Individual[] Individuals
        {
            get
            {
                return individuals;
            }
        }

        internal static Func<double[], int> FitnessFunction
        {
            get
            {
                return fitnessFunction;
            }
            set
            {
                fitnessFunction = value;
            }
        }

        public int PopulationSize
        {
            get
            {
                return individuals.Length;
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void GeneratePopulation(GeneInfo geneInfo)
        {
            Random random = new Random();

            for(int i = 0; i < individuals.Length; i++)
            {
                Individual individual = new Individual(geneInfo, random);
                individuals[i] = individual;
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Calculates the fitness of each individual and returns the fittest.
        /// </summary>
        public Individual GetFittestIndividual()
        {
            if (individuals.Length > 0)
            {
                Individual fittest = individuals[0];

                for (int i = 0; i < PopulationSize; i++)
                {
                    if (fittest.GetFitness() <= individuals[i].GetFitness())
                    {
                        fittest = individuals[i];
                    }
                }

                return fittest;
            }
            else
            {
                throw new Exception("No individuals in population!");
            }
        }

        #endregion Public Methods
    }
}
