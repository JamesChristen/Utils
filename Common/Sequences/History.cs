using System.Text;

namespace Common.Sequences
{
    public class History<T>
    {
        private readonly SortedList<DateTime, T> _hist;

        public History()
        {
            _hist = new SortedList<DateTime, T>();
        }

        public History(DateTime knownFrom, T value)
            : this(new Dictionary<DateTime, T> { { knownFrom, value } })
        {
        }

        public History(Dictionary<DateTime, T> dict)
        {
            _hist = new SortedList<DateTime, T>(dict);
        }

        public IEnumerable<DateTime> Keys => _hist.Keys;

        public IEnumerable<T> Values => _hist.Values;

        public IEnumerable<KeyValuePair<DateTime, T>> Data => _hist;

        public bool HasValueKnownOn(DateTime knownFrom) => _hist.Where(x => x.Key <= knownFrom).Any();

        public T GetValueKnownOn(DateTime knownFrom)
        {
            IEnumerable<KeyValuePair<DateTime, T>> values = _hist.Where(x => x.Key <= knownFrom).OrderByDescending(x => x.Key);
            if (!values.Any())
            {
                return default;
            }
            else
            {
                return values.First().Value;
            }
        }

        public T Latest => _hist.Count == 0 ? default : _hist.Last().Value;

        public DateTime? LatestKnownFrom => _hist.Count == 0 ? null : _hist.Last().Key;

        public History<T> Add(DateTime knownFrom, T item)
        {
            _hist.Add(knownFrom, item);
            return this;
        }

        public History<T> AddOrUpdate(DateTime knownFrom, T item)
        {
            if (_hist.ContainsKey(knownFrom))
            {
                _hist[knownFrom] = item;
            }
            else
            {
                _hist.Add(knownFrom, item);
            }
            return this;
        }

        public Dictionary<DateTime, T> ToDictionary()
        {
            return _hist.ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<DateTime, S> ToDictionary<S>(Func<T, S> transform)
        {
            ArgumentNullException.ThrowIfNull(transform);

            return _hist.ToDictionary(x => x.Key, x => transform(x.Value));
        }

        public override string ToString()
        {
            KeyValuePair<DateTime, T>[] arr = _hist.ToArray();
            StringBuilder output = new();
            for (int i = 0; i < arr.Length; i++)
            {
                output.AppendLine($"Known {arr[i].Key:G} => {(i < arr.Length - 1 ? arr[i + 1].Key.ToString("G") : "")}: {arr[i].Value}");
            }
            return output.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj is not History<T> hist)
            {
                return false;
            }
            return _hist.SequenceEqual(hist._hist);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); // Don't need to override this, only to prevent compiler warnings
        }
    }
}
