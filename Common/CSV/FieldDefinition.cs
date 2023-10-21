namespace Common.CSV
{
    public class FieldDefinition
    {
        private readonly Dictionary<Type, Func<string, object>> _parsers;

        public int ColNum { get; }

        public FieldDefinition(int colNum, params Type[] types)
        {
            if (!types.Any())
            {
                throw new ArgumentException($"No possible types provided for column at {colNum}");
            }

            ColNum = colNum;
            _parsers = types.ToDictionary(x => x, x => (Func<string, object>)null);
        }

        public FieldDefinition(int colNum, Dictionary<Type, Func<string, object>> parsers)
        {
            if (!parsers.Any())
            {
                throw new ArgumentException($"No possible types provided for column at {colNum}");
            }

            ColNum = colNum;
            _parsers = parsers;
        }

        public FieldDefinition(int colNum, Type type, Func<string, object> parser)
        {
            ColNum = colNum;
            _parsers = new Dictionary<Type, Func<string, object>> { { type, parser } };
        }

        public dynamic ParseValue(string value)
        {
            foreach (KeyValuePair<Type, Func<string, object>> set in _parsers)
            {
                try
                {
                    if (set.Value == null)
                    {
                        return ValueParsers.GetParsedValue(value, set.Key);
                    }
                    return set.Value(value);
                }
                catch (ArgumentException) { } // Swallow and throw exception at the end if no parsers work
            }
            throw new ArgumentException($"Could not parse {value} to any of the types: {string.Join(", ", _parsers.Keys.Select(x => x.Name))}");
        }
    }
}
