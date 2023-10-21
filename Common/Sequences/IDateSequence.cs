namespace Common.Sequences
{
    public interface IDateSequence<T> : IEnumerable<KeyValuePair<DateTime, T>>
    {
        SequenceOptions Options { get; set; }

        /// <summary>
        /// Creates a new <see cref="IDateSequence{S}"/> using the same <see cref="SequenceOptions"/>
        /// </summary>
        IDateSequence<S> CreateWithSameOptions<S>();

        /// <summary>
        /// Adds the key value pair to the <see cref="IDateSequence{T}"/>.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="key"/> already exists.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="key"/> is invalid, classified by:<br/>
        /// - <see cref="Options.IncludeWeekends"/> is false and <paramref name="key"/> is a weekend<br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Daily"/>
        /// and <paramref name="key"/>.TimeOfDay does not equal <see cref="FirstKey.Value.TimeOfDay"/><br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Intraday"/>
        /// and <paramref name="key"/>.TimeOfDay falls outside of <see cref="Options"/> bar range
        /// </summary>
        /// <exception cref="ArgumentException"/>
        void Add(DateTime key, T value);

        /// <summary>
        /// Adds or updates the key value pair to the <see cref="IDateSequence{T}"/>.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="key"/> is invalid, classified by:<br/>
        /// - <see cref="Options.IncludeWeekends"/> is false and <paramref name="key"/> is a weekend<br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Daily"/>
        /// and <paramref name="key"/>.TimeOfDay does not equal <see cref="FirstKey.Value.TimeOfDay"/><br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Intraday"/>
        /// and <paramref name="key"/>.TimeOfDay falls outside of <see cref="Options"/> bar range
        /// </summary>
        /// <exception cref="ArgumentException"/>
        void AddOrUpdate(DateTime key, T value);

        /// <summary>
        /// Adds the set of key value pairs to the <see cref="IDateSequence{T}"/>.<br/>
        /// Throws <see cref="ArgumentException"/> if any of the keys in <paramref name="range"/> already exists.<br/>
        /// Throws <see cref="ArgumentException"/> if any key in <paramref name="range"/> is invalid, classified by:<br/>
        /// - <see cref="Options.IncludeWeekends"/> is false and the key is a weekend<br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Daily"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> does not equal <see cref="FirstKey.Value.TimeOfDay"/><br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Intraday"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> falls outside of <see cref="Options"/> bar range
        /// </summary>
        /// <exception cref="ArgumentException"/>
        void AddRange(Dictionary<DateTime, T> range);

        /// <summary>
        /// Adds or updates each key value pair to the <see cref="IDateSequence{T}"/>.<br/>
        /// Throws <see cref="ArgumentException"/> if any date is invalid, classified by:<br/>
        /// - <see cref="Options.IncludeWeekends"/> is false and the key is a weekend<br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Daily"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> does not equal <see cref="FirstKey.Value.TimeOfDay"/><br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Intraday"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> falls outside of <see cref="Options"/> bar range
        /// </summary>
        /// <exception cref="ArgumentException"/>
        void AddOrUpdateRange(Dictionary<DateTime, T> range);

        /// <summary>
        /// Returns whether the <see cref="IDateSequence{T}"/> has a value on <paramref name="key"/>
        /// </summary>
        bool ContainsKey(DateTime key);

        /// <summary>
        /// Returns whether the <see cref="IDateSequence{T}"/> has a value on <paramref name="key"/>, 
        /// or if the value can be determined using <see cref="SequenceOptions.MissingData"/>
        /// </summary>
        bool ContainsKeyOrDefault(DateTime key);

        /// <summary>
        /// Returns the value stored at <paramref name="key"/>.<br/>
        /// If a value is not found for <paramref name="key"/>, the <see cref="SequenceOptions.MissingData"/>
        /// behaviour is used to return a default value. If no default value can be determined a
        /// <see cref="KeyNotFoundException"/> will be thrown
        /// </summary>
        /// <exception cref="KeyNotFoundException"/>
        T this[DateTime key] { get; }

        /// <summary>
        /// Returns the value stored at <paramref name="key"/>, 
        /// or the <paramref name="defaultValue"/> if no data can be found.
        /// </summary>
        T GetValueOrDefault(DateTime key, T defaultValue);

        /// <summary>
        /// Returns the value at the earliest date, 
        /// or the <paramref name="defaultValue"/> if no data is present in the sequence.
        /// </summary>
        T FirstValueOrDefault(T defaultValue = default);

        /// <summary>
        /// Returns the value at the latest date, 
        /// or the <paramref name="defaultValue"/> if no data is present in the sequence.
        /// </summary>
        T LastValueOrDefault(T defaultValue = default);

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
        /// Returns whether there are no key/value pairs contained in the sequence.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns a collection of all of the dates contained in the sequence.
        /// </summary>
        IEnumerable<DateTime> Keys { get; }

        /// <summary>
        /// Returns a collection of all of the values contained in the sequence.
        /// </summary>
        IEnumerable<T> Values { get; }

        /// <summary>
        /// Sets the start of the sequence to be <paramref name="date"/>. If <paramref name="date"/> is less than <see cref="FirstKey"/>
        /// the value is set to the default of <see cref="T"/>, otherwise the series has the start truncated.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="date"/> is invalid, classified by:<br/>
        /// - <see cref="Options.IncludeWeekends"/> is false and the key is a weekend<br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Daily"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> does not equal <see cref="FirstKey.Value.TimeOfDay"/><br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Intraday"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> falls outside of <see cref="Options"/> bar range
        /// </summary>
        /// <exception cref="ArgumentException"/>
        IDateSequence<T> SetStart(DateTime date);

        /// <summary>
        /// Sets the end of the sequence to be <paramref name="date"/>. If <paramref name="date"/> is greater than <see cref="LastKey"/>
        /// the value is set to the default of <see cref="T"/>, otherwise the series has the end truncated.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="date"/> is invalid, classified by:<br/>
        /// - <see cref="Options.IncludeWeekends"/> is false and the key is a weekend<br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Daily"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> does not equal <see cref="FirstKey.Value.TimeOfDay"/><br/>
        /// - <see cref="Options.Frequency"/> is <see cref="SequenceFrequency.Intraday"/>
        /// and the key <see cref="DateTime.TimeOfDay"/> falls outside of <see cref="Options"/> bar range
        /// </summary>
        /// <exception cref="ArgumentException"/>
        IDateSequence<T> SetEnd(DateTime date);

        /// <summary>
        /// Returns a new sequence of all the values in between <paramref name="startDate"/> and <paramref name="endDate"/>.<br/>
        /// The start of the sequence will be Max(<paramref name="startDate"/>, <see cref="FirstKey"/>), and the end will be
        /// Min(<paramref name="endDate"/>, <see cref="LastKey"/>)
        /// </summary>
        IDateSequence<T> InRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns a new sequence with every date required being populated with a value already present or the default of <see cref="T"/>.<br/>
        /// Fill type is determined by <see cref="Options.Frequency"/>.<br/>
        /// Throws <see cref="NotImplementedException"/> if no fill implementation for <see cref="Options.Frequency"/>
        /// </summary>
        /// <exception cref="NotImplementedException"/>
        IDateSequence<T> FillMissingDates();

        /// <summary>
        /// Adds the <paramref name="shift"/> to each key in the sequence
        /// </summary>
        IDateSequence<T> ShiftDates(TimeSpan shift);

        /// <summary>
        /// Filters the sequence by <paramref name="filter"/>.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="filter"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IDateSequence<T> Filter(Func<T, bool> filter);

        /// <summary>
        /// Applies <paramref name="transformer"/> to each value in the sequence.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="transformer"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IDateSequence<S> Transform<S>(Func<T, S> transformer);

        /// <summary>
        /// Applies <paramref name="transformer"/> to each value in the sequence.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="transformer"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IDateSequence<S> Transform<S>(Func<DateTime, T, S> transformer);

        /// <summary>
        /// Takes the union of dates of this sequence and <paramref name="seq"/>, and applies the <see cref="JoinOptions{T1, T2, S}.JoinBehaviour"/>
        /// to the two elements of each sequence on each date in the union of dates.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="seq"/> is null<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="joinOptions"/> is null<br/>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IDateSequence<S> Join<T2, S>(IDateSequence<T2> seq, JoinOptions<T, T2, S> joinOptions);

        /// <summary>
        /// Returns an <see cref="IHistorySequence{T}"/> where each item in sequence has the KnownFrom set as the date on that item.
        /// </summary>
        IHistorySequence<T> ToHistoryUsingRefDate();

        /// <summary>
        /// Returns an <see cref="IHistorySequence{T}"/> where each item in sequence has the <paramref name="knownFrom"/> specified.
        /// </summary>
        IHistorySequence<T> ToHistoryKnownFrom(DateTime knownFrom);

        /// <summary>
        /// Returns an <see cref="IHistorySequence{T}"/> where each item in sequence has the KnownFrom set from the <paramref name="knownFromProvider"/>.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="knownFromProvider"/> is null<br/>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        IHistorySequence<T> ToHistory(Func<DateTime, T, DateTime> knownFromProvider);

        void Print(Action<string> print, string separator = "\r\n");
        bool SequenceEqual(IDateSequence<T> other);
    }
}
