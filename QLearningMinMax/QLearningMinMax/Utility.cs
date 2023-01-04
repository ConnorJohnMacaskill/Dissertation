using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLearning
{
    public static class Utility
    {
        private static Random random = new Random();

        public static int RandomInteger(int min, int max)
        {
            return random.Next(min, max);
        }

        public static double RandomDouble()
        {
            return random.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            double value = random.NextDouble();

            return min + (value * (max - min));
        }
    }
}
