using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class GeneInfo
    {
        private GeneType geneType;
        private int length;
        private int minValue;
        private int maxValue;
        
        public GeneInfo(GeneType geneType, int length, int minValue, int maxValue)
        {
            this.geneType = geneType;
            this.length = length;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public GeneType GeneType
        {
            get
            {
                return geneType;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int MinValue
        {
            get
            {
                return minValue;
            }
        }

        public int MaxValue
        {
            get
            {
                return maxValue;
            }
        }

        public override string ToString()
        {
            return string.Format("Genetic Information - GeneType : {0}, Length : {1}, Min : {2}, Max : {3}", geneType.ToString(), length, minValue, maxValue);
        }
    }
}
