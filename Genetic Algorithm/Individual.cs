using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Individual
    {
        private double[] genes;
        private int fitness;
        private GeneInfo geneInfo;

        public Individual(GeneInfo geneInfo)
        {
            genes = new double[geneInfo.Length];
            this.geneInfo = geneInfo;
        }

        public Individual(GeneInfo geneInfo, Random random)
        {
            genes = new double[geneInfo.Length];
            this.geneInfo = geneInfo;

            GenerateIndividual(geneInfo, random);
        }

        #region Public Properties

        public double[] Genes
        {
            get
            {
                return genes;
            }
        }

        public GeneInfo GeneInfo
        {
            get
            {
                return geneInfo;
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Generates a new individual with random genes.
        /// </summary>
        private void GenerateIndividual(GeneInfo geneInfo, Random random)
        {
            for(int i = 0; i < genes.Length; i++)
            {
                switch (geneInfo.GeneType)
                {
                    case GeneType.INT:
                        genes[i] = random.Next(geneInfo.MinValue, geneInfo.MaxValue);
                        break;
                    case GeneType.DOUBLE:
                        genes[i] = random.NextDouble() * (geneInfo.MaxValue - geneInfo.MinValue) + geneInfo.MinValue;
                        break;
                    default:
                        throw new Exception("GeneType incorrectly defined!");
                }

            }
        }

        #endregion Private Methods

        #region Public Methods

        public int GetFitness()
        {
            if(fitness == 0)
            {
                fitness = Population.FitnessFunction(genes);
            }

            return fitness;
        }

        #endregion Public Methods
    }
}
