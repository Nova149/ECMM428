using System;
using System.Collections.Generic;
using System.Text;

namespace ECMM428
{
    public static class Util
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            //Use a random seed
            Random rand = new Random();
            Shuffle(list, rand.Next());
        }
        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            Random rng = new Random(seed);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static T GetRandom<T>(this IList<T> list)
        {
            //Use a random seed
            Random rand = new Random();
            return GetRandom(list, rand.Next());
        }
        public static T GetRandom<T>(this IList<T> list, int seed)
        {
            Random rng = new Random(seed);
            return list[rng.Next(0, list.Count)];
        }
        public static double GetAverage(double[] array)
        {
            double total = 0;
            foreach (double d in array) total += d;
            return total / array.Length;
        }
        public static double GetAverage(int[] array)
        {
            double total = 0;
            foreach (double d in array) total += d;
            return total / array.Length;
        }
        public static double GetStandardDeviation(double[] array)
        {
            double sd = 0;
            double mean = GetAverage(array);
            foreach (double i in array)
            {
                sd += Math.Pow((i - mean),2);
            }
            sd = Math.Sqrt(sd / array.Length);
            return sd;
        }
    }
}
