using System;
using System.IO;

namespace Common.Test
{
    public static class FileHelpers
    {
        public static string ReadAllText(string filepath)
        {
            return File.ReadAllText(Path.Combine(Environment.CurrentDirectory, filepath));
        }

        public static string ReadAllText(string dir, string filepath)
        {
            return ReadAllText(Path.Combine(dir, filepath));
        }
    }
}
