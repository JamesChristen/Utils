using Common.Sequences;

namespace Common.CSV
{
    public interface ICsvWriter
    {
        void WriteCsv(string filepath, Dictionary<string, ITimeSequence> dict);
        void WriteCsv<T>(string filepath, Dictionary<string, IDateSequence<T>> dict);
        void WriteCsv<T>(string filepath, string header, IEnumerable<T> values);
    }
}
