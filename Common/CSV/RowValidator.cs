using Common.Validation;

namespace Common.CSV
{
    public class RowValidator
    {
        protected readonly CsvDefinition _csvDefinition;
        protected readonly int _rowNum;
        protected readonly string[] _values;
        protected readonly List<string> _errors = new List<string>();

        public RowValidator(CsvDefinition csvDefinition, int rowNum, string line)
        {
            Validate.That
                    .IsNotNull(csvDefinition, nameof(csvDefinition))
                    .IsPositive(rowNum, nameof(rowNum))
                    .IsNotNullOrEmpty(line, nameof(line))
                    .Check();

            _csvDefinition = csvDefinition;
            _rowNum = rowNum;
            _values = line.Split(csvDefinition.ColDelimiter, StringSplitOptions.None);
        }

        public bool IsValid(out string errors)
        {
            if (_errors.Any())
            {
                // Add row information in too
                errors = string.Join(Environment.NewLine, _errors.Select(x => $"Error on row {_rowNum}: {x}"));
                return false;
            }
            errors = string.Empty;
            return true;
        }

        public bool IsRowValid()
        {
            return _values.Length == _csvDefinition.FieldDefinitions.Count;
        }

        public dynamic GetParsedValue(string header)
        {
            try
            {
                if (_csvDefinition.FieldDefinitions.TryGetValue(header, out FieldDefinition fieldDefinition))
                {
                    string value = GetValue(header);
                    return fieldDefinition.ParseValue(value);
                }
                throw new ArgumentException($"{header} does not have a field definition");
            }
            catch (ArgumentException argEx)
            {
                _errors.Add(argEx.Message);
                return default;
            }
        }

        public T GetEnum<T>(string header) where T : struct, Enum
        {
            try
            {
                string str = GetValue(header);
                return ValueParsers.ParseEnum<T>(str);
            }
            catch (ArgumentException argEx)
            {
                _errors.Add(argEx.Message);
                return default;
            }
        }

        public T? GetNullableEnum<T>(string header) where T : struct, Enum
        {
            try
            {
                string str = GetValue(header);
                return ValueParsers.ParseNullableEnum<T>(str);
            }
            catch (ArgumentException argEx)
            {
                _errors.Add(argEx.Message);
                return default;
            }
        }

        public bool HasValue(string header)
        {
            if (_csvDefinition.FieldDefinitions.TryGetValue(header, out FieldDefinition fieldDefinition))
            {
                return _values.Length - 1 >= fieldDefinition.ColNum;
            }
            throw new ArgumentException($"{header} does not exist in file");
        }

        private string GetValue(string header)
        {
            if (_csvDefinition.FieldDefinitions.TryGetValue(header, out FieldDefinition fieldDefinition))
            {
                int colNum = fieldDefinition.ColNum;
                if (_values.Length - 1 >= colNum)
                {
                    return _values[colNum];
                }
                throw new IndexOutOfRangeException($"There is no data for {header}");
            }
            throw new ArgumentException($"{header} does not exist in file");
        }
    }
}
