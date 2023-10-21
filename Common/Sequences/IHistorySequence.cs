namespace Common.Sequences
{
    public interface IHistorySequence<T> : IEnumerable<KeyValuePair<DateTime, History<T>>>
    {
        SequenceOptions Options { get; set; }

        /// <summary>
        /// Adds or updates the key value pair to the <see cref="IHistorySequence{T}"/>.<br/>
        /// </summary>
        void Add(DateTime refDate, DateTime knownFrom, T value);

        /// <summary>
        /// Adds or updates the key value pair to the <see cref="IHistorySequence{T}"/>.<br/>
        /// </summary>
        void AddOrUpdate(DateTime refDate, DateTime knownFrom, T value);

        /// <summary>
        /// Adds or updates each key value pair of <paramref name="values"/> to the <see cref="History{T}"/> on <paramref name="refDate"/>.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="values"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        void Add(DateTime refDate, Dictionary<DateTime, T> values);

        /// <summary>
        /// Adds or updates each key value pair of <paramref name="values"/> to the <see cref="History{T}"/> on <paramref name="refDate"/>.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="values"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        void AddOrUpdate(DateTime refDate, Dictionary<DateTime, T> values);

        /// <summary>
        /// Returns whether the sequence has a value on <paramref name="key"/>
        /// </summary>
        bool ContainsKey(DateTime key);

        /// <summary>
        /// Returns the <see cref="History{T}"/> on <paramref name="refDate"/>, or null if not present
        /// </summary>
        History<T> this[DateTime refDate] { get; }

        /// <summary>
        /// Returns the value on <paramref name="refDate"/>, that was known at <paramref name="knownFrom"/>.<br/>
        /// If a value is not found for <paramref name="refDate"/>, the <see cref="SequenceOptions.MissingData"/>
        /// behaviour is used to return a default value. If no default value can be determined a
        /// <see cref="KeyNotFoundException"/> will be thrown
        /// </summary>
        /// <exception cref="KeyNotFoundException"/>
        T this[DateTime refDate, DateTime knownFrom] { get; }

        /// <summary>
        /// Returns the earliest date, or null if no data is present in the sequence.
        /// </summary>
        DateTime? FirstKey { get; }

        /// <summary>
        /// Returns the latest date, or null if no data is present in the sequence.
        /// </summary>
        DateTime? LastKey { get; }

        /// <summary>
        /// Returns the number of key/value pairs contained in the sequence.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns a collection of all of the dates contained in the sequence.
        /// </summary>
        IEnumerable<DateTime> Keys { get; }

        /// <summary>
        /// Returns a collection of all of the values contained in the sequence.
        /// </summary>
        IEnumerable<History<T>> Values { get; }

        /// <summary>
        /// Adds the <paramref name="shift"/> to each key in the sequence
        /// </summary>
        IHistorySequence<T> ShiftDates(TimeSpan shift);

        /// <summary>
        /// Filters the sequence by <paramref name="filter"/>.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="filter"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IHistorySequence<T> Filter(Func<T, bool> filter);

        /// <summary>
        /// Applies <paramref name="transformer"/> to each value in the history of each item in the sequence.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="transformer"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IHistorySequence<S> Transform<S>(Func<T, S> transformer);

        /// <summary>
        /// Returns a new sequence of all the values in between <paramref name="startDate"/> and <paramref name="endDate"/>.<br/>
        /// The start of the sequence will be Max(<paramref name="startDate"/>, <see cref="FirstKey"/>), and the end will be
        /// Min(<paramref name="endDate"/>, <see cref="LastKey"/>)
        /// </summary>
        IHistorySequence<T> InRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns an <see cref="IDateSequence{T}"/> of all the values known on <paramref name="date"/> for each date in the sequence.<br/>
        /// If a value is not known on a certain date that date will not be contained in the resulting <see cref="IDateSequence{T}"/>
        /// </summary>
        IDateSequence<T> GetSequenceKnownOnDate(DateTime date);

        /// <summary>
        /// Returns an <see cref="IDateSequence{T}"/> of all the latest values for each date in the sequence.
        /// </summary>
        IDateSequence<T> GetLatestSequence();

        /// <summary>
        /// Returns every value in the sequence, flattened to a single <see cref="IEnumerable{T}"/>
        /// </summary>
        IEnumerable<T> Flatten();
    }
}
