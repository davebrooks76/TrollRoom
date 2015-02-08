using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TrollRoom
{
    public static class ThreadLocalRandom
    {
        private static readonly Random globalRandom = new Random();
        private static readonly object globalLock = new object();
        
        private static readonly ThreadLocal<Random> threadRandom = new ThreadLocal<Random>(NewRandom);
        
        public static Random NewRandom()
        {
            lock (globalLock)
            {
                return new Random(globalRandom.Next());
            }
        }
        
        public static Random Instance { get { return threadRandom.Value; } }

        public static int Next()
        {
            return Instance.Next();
        }

        public static int Next(int maxValue)
        {
            return Instance.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return Instance.Next(minValue, maxValue);
        }

        public static double NextDouble()
        {
            return Instance.NextDouble();
        }

        public static void NextBytes(byte[] buffer)
        {
            Instance.NextBytes(buffer);
        } 
    }
}
