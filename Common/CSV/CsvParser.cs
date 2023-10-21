using CsvHelper;
using System.Globalization;

namespace Common.CSV
{
    public abstract class CsvParser<T> : ICsvParser<T>
    {
        public virtual IEnumerable<T> ParseFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException($"File '{filepath}' not found");
            }

            if (!CanRead(filepath))
            {
                throw new AccessViolationException($"File '{filepath}' cannot be read");
            }
            string csvString = File.ReadAllText(filepath);
            return ParseText(csvString);
        }

        public virtual IEnumerable<T> ParseText(string csvString)
        {
            using (StringReader textReader = new StringReader(csvString))
            using (CsvReader csvr = new CsvReader(textReader, CultureInfo.InvariantCulture))
            {
                return csvr.GetRecords<T>();
            };
        }

        private bool CanRead(string filepath)
        {
            using var fs = new FileStream(filepath, FileMode.Open);
            return fs.CanRead;
        }
    }
}
