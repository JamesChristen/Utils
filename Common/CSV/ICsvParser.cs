namespace Common.CSV
{
    public interface ICsvParser<T>
    {
        IEnumerable<T> ParseText(string csvString);
        IEnumerable<T> ParseFile(string filepath);
    }
}
