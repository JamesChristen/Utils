using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Test
{
    public static class Utils
    {
        public static string ReadAllText(string filepath)
        {
            return File.ReadAllText(Path.Combine(Environment.CurrentDirectory, filepath));
        }

        public static string ReadAllText(string dir, string filepath)
        {
            return ReadAllText(Path.Combine(dir, filepath));
        }

        public static bool AreEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> observed)
        {
            if (expected is null && observed is null)
            {
                return true;
            }
            else if (expected is null || observed is null)
            {
                return false;
            }
            else if (expected.Count() != observed.Count())
            {
                return false;
            }

            return expected.Union(observed).Count() == expected.Count();
        }

        public static bool AreEquivalent<T>(IEnumerable<IEnumerable<T>> expected, IEnumerable<IEnumerable<T>> observed)
        {
            if (expected is null && observed is null)
            {
                return true;
            }
            else if (expected is null || observed is null)
            {
                return false;
            }
            else if (expected.Count() != observed.Count())
            {
                return false;
            }

            for (int i = 0; i < expected.Count(); i++)
            {
                if (!AreEquivalent(expected.ElementAt(i), observed.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
