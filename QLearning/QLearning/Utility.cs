using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning
{
    public static class Utility
    {
        private static Random random = new Random();

        /// <summary>
        /// Returns a random integer between the min and max value.
        /// </summary>
        public static int RandomInteger(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Returns a random double between 0.0 and 1.0;
        /// </summary>
        public static double RandomDouble()
        {
            return random.NextDouble();
        }

        /// <summary>
        /// Returns a random double between the min and max value.
        /// </summary>
        public static double RandomDouble(double min, double max)
        {
            double value = random.NextDouble();

            return min + (value * (max - min));
        }
    }
}
