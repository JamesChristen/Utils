namespace Common.CsvParser
{
    public class CsvDefinition
    {
        public string RowDelimiter { get; }
        public string ColDelimiter { get; }

        public string Header => string.Join(ColDelimiter, FieldDefinitions.OrderBy(x => x.Value.ColNum).Select(x => x.Key));

        public Dictionary<string, FieldDefinition> FieldDefinitions { get; }

        public CsvDefinition(Dictionary<string, FieldDefinition> fieldDefinitions, string colDelimiter = ",", string rowDelimiter = "\r\n")
        {
            if (fieldDefinitions == null || fieldDefinitions.Count == 0)
            {
                throw new ArgumentException($"{nameof(fieldDefinitions)} cannot be null or empty");
            }
            FieldDefinitions = fieldDefinitions;

            if (string.IsNullOrEmpty(colDelimiter))
            {
                throw new ArgumentException($"{nameof(colDelimiter)} cannot be null or empty");
            }
            ColDelimiter = colDelimiter;

            if (string.IsNullOrEmpty(rowDelimiter))
            {
                throw new ArgumentException($"{nameof(rowDelimiter)} cannot be null or empty");
            }
            RowDelimiter = rowDelimiter;
        }

        public void ValidateHeader(string header)
        {
            if (!header.Equals(Header, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Header does not match expected: {Header}. Received: {header}");
            }
        }
    }
}
