using Common.Sequences;

namespace Common.CSV
{
    internal class CsvWriter : ICsvWriter
    {
        public void WriteCsv(string filepath, Dictionary<string, ITimeSequence> dict)
        {
            WriteCsv(filepath, dict.ToDictionary(x => x.Key, x => x.Value as IDateSequence<decimal>));
        }

        public void WriteCsv<T>(string filepath, Dictionary<string, IDateSequence<T>> dict)
        {
            using StreamWriter streamWriter = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write));
            string header = $"Date,{string.Join(",", dict.Keys)}";
            streamWriter.Write(header);
            streamWriter.WriteLine();

            List<DateTime> dates = dict.Values.SelectMany(x => x.Keys).Distinct().OrderBy(x => x).ToList();
            foreach (DateTime date in dates)
            {
                string line = $"{date:yyyy/MM/dd},{string.Join(",", dict.Select(x => x.Value.ContainsKey(date) ? x.Value[date].ToString() : ""))}";
                streamWriter.Write(line);
                streamWriter.WriteLine();
            }
            streamWriter.Flush();
        }

        public void WriteCsv<T>(string filepath, string header, IEnumerable<T> values)
        {
            using StreamWriter streamWriter = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write));
            streamWriter.Write(header);
            streamWriter.WriteLine();

            foreach (T value in values)
            {
                streamWriter.Write(value.ToString());
                streamWriter.WriteLine();
            }
            streamWriter.Flush();
        }
    }
}
