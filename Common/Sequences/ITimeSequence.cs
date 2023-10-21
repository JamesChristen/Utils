namespace Common.Sequences
{
    public interface ITimeSequence : IDateSequence<decimal>
    {
        /// <summary>
        /// Returns a new sequence of all the values in between <paramref name="start"/> and <paramref name="end"/>.<br/>
        /// The start of the sequence will be Max(<paramref name="start"/>, <see cref="FirstKey"/>), and the end will be
        /// Min(<paramref name="end"/>, <see cref="LastKey"/>)
        /// </summary>
        new ITimeSequence InRange(DateTime start, DateTime end);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of the absolute value of each item
        /// </summary>
        ITimeSequence Absolute();

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of the negative value of each item
        /// </summary>
        ITimeSequence Negate();

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of the sum of each value in the current sequence and <paramref name="seq"/>.<br/>
        /// If <paramref name="seq"/> has no value on a specific date, the default of 0 is added.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="seq"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        ITimeSequence Add(ITimeSequence seq);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of the sum of each value in the current sequence and the negative of <paramref name="seq"/>.<br/>
        /// If <paramref name="seq"/> has no value on a specific date, the default of 0 is subtracted.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="seq"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        ITimeSequence Subtract(ITimeSequence seq);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of each value in the current sequence divided by <paramref name="denominator"/>.<br/>
        /// Throws <see cref="DivideByZeroException"/> if <paramref name="denominator"/> is 0
        /// </summary>
        /// <see cref="DivideByZeroException"/>
        ITimeSequence DivideBy(decimal denominator);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of each value in the current sequence divided by each value of <paramref name="seq"/> on the relevant date.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="seq"/> does not contain a value on any of the dates in the current sequence.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="seq"/> is null.<br/>
        /// Throws <see cref="DivideByZeroException"/> if <paramref name="denominator"/> is 0
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DivideByZeroException"/>
        ITimeSequence DivideBy(ITimeSequence seq);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of each value in the current sequence multiplied by <paramref name="multiplier"/>.<br/>
        /// </summary>
        ITimeSequence MultiplyBy(decimal multiplier);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of each value in the current sequence multiplied by each value of <paramref name="seq"/> on the relevant date.<br/>
        /// Throws <see cref="ArgumentException"/> if <paramref name="seq"/> does not contain a value on any of the dates in the current sequence.<br/>
        /// Throws <see cref="ArgumentNullException"/> if <paramref name="seq"/> is null.<br/>
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        ITimeSequence MultiplyBy(ITimeSequence seq);

        /// <summary>
        /// Returns a new <see cref="ITimeSequence"/> of the square root of each value in the current sequence.<br/>
        /// Throws <see cref="InvalidOperationException"/> if any of the values are negative
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        ITimeSequence SquareRoot();

        ITimeSequence MeanExpanding();
        ITimeSequence MeanWindowed(WindowLength windowLength);

        ITimeSequence StandardDeviationExpanding();
        ITimeSequence StandardDeviationWindowed(WindowLength windowLength);
        ITimeSequence StandardDeviationPopulationExpanding();
        ITimeSequence StandardDeviationPopulationWindowed(WindowLength windowLength);

        ITimeSequence VarianceExpanding();
        ITimeSequence VarianceWindowed(WindowLength windowLength);
        ITimeSequence VariancePopulationExpanding();
        ITimeSequence VariancePopulationWindowed(WindowLength windowLength);

        ITimeSequence ZScoreExpanding();
        ITimeSequence ZScoreWindowed(WindowLength windowLength);
    }

    public static class ITimeSequenceExtensions
    {
        public static ITimeSequence ToTimeSequence(this Dictionary<DateTime, decimal> items)
            => new TimeSequence(items);
    }
}
