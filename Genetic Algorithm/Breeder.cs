using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Breeder
    {
        private double uniformRate = 0.5;
        private double mutationRate = 0.15;
        private int tournamentSize = 5;
        private bool elitism = true;
        private Random random;

        public Breeder(double uniformRate = 0.5, double mutationRate = 0.15, int tournamentSize = 5, bool elitism = true)
        {
            this.uniformRate = uniformRate;
            this.mutationRate = mutationRate;
            this.tournamentSize = tournamentSize;
            this.elitism = elitism;
            this.random = new Random();
        }

        public Population Evolve(Population population)
        {
            Individual[] individuals = new Individual[population.PopulationSize];

            if(elitism)
            {
                individuals[0] = population.GetFittestIndividual();
            }

            for(int i = Convert.ToInt32(elitism); i < population.PopulationSize; i++)
            {
                Individual father = TournamentSelection(population);
                Individual mother = TournamentSelection(population);

                Individual child = Crossover(father, mother);
                individuals[i] = child;
            }

            for (int i = Convert.ToInt32(elitism); i < population.PopulationSize; i++)
            {
                Mutate(individuals[i]);
            }

            return new Population(individuals);
        }

        private Individual Crossover(Individual father, Individual mother)
        {

            Individual child = new Individual(father.GeneInfo);

            for (int i = 0; i < father.Genes.Length; i++)
            {
                if (random.NextDouble() <= uniformRate)
                {
                    child.Genes[i] = father.Genes[i];
                }
                else
                {
                    child.Genes[i] = mother.Genes[i];
                }
            }

            return child;
        }

        private void Mutate(Individual individual)
        {
            for (int i = 0; i < individual.Genes.Length; i++)
            {
                if (random.NextDouble() <= mutationRate)
                {
                    GeneInfo geneInfo = individual.GeneInfo;

                    switch (geneInfo.GeneType)
                    {
                        case GeneType.INT:
                            individual.Genes[i] = random.Next(geneInfo.MinValue, geneInfo.MaxValue);
                            break;
                        case GeneType.DOUBLE:
                            individual.Genes[i] = random.NextDouble() * (geneInfo.MaxValue - geneInfo.MinValue) + geneInfo.MinValue;
                            break;
                        default:
                            throw new Exception("GeneType incorrectly defined!");
                    }
                }
            }
        }

        private Individual TournamentSelection(Population population)
        {
            Individual[] individuals = new Individual[tournamentSize];

            for(int i = 0; i < tournamentSize; i++)
            {
                int randomIndex = random.Next(0, population.PopulationSize);
                individuals[i] = population.Individuals[randomIndex];
            }

            Population tournament = new Population(individuals);
            return tournament.GetFittestIndividual();
        }
    }
}
