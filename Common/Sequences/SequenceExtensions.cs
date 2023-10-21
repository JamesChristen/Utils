namespace Common.Sequences
{
    public static class SequenceExtensions
    {
        public static IDateSequence<T> ToSequence<T>(this IEnumerable<T> items, Func<T, DateTime> keySelector)
        {
            return items.ToSequence(keySelector, x => x);
        }

        public static IDateSequence<S> ToSequence<T, S>(this IEnumerable<T> items, Func<T, DateTime> keySelector, Func<T, S> valueSelector)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(valueSelector);

            var groupedByKey = items.GroupBy(x => keySelector(x));
            IEnumerable<DateTime> duplicateKeys = groupedByKey.Where(x => x.Count() > 1).Select(x => x.Key);
            if (duplicateKeys.Any())
            {
                throw new ArgumentException($"Duplicate keys in set:\n{string.Join(',', duplicateKeys.Select(x => $"{x:G}"))}");
            }

            Dictionary<DateTime, S> set =
                groupedByKey.Select(x => new KeyValuePair<DateTime, T>(x.Key, x.Single()))
                            .ToDictionary(x => x.Key, x => valueSelector(x.Value));

            return new DateSequence<S>(set);
        }

        public static ITimeSequence ToTimeSequence<T>(this IDateSequence<T> seq, Func<T, decimal> transform)
        {
            ArgumentNullException.ThrowIfNull(seq);

            return new TimeSequence(seq.Transform(transform));
        }
    }
}
