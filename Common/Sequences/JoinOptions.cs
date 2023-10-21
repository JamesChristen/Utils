namespace Common.Sequences
{
    public class JoinOptions<T1, T2, S>
    {
        public MissingDataBehaviour MissingData { get; set; } = MissingDataBehaviour.DefaultValue;
        public Func<T1, T2, S> JoinBehaviour { get; set; }
        public S DefaultValue { get; set; }

        public JoinOptions(MissingDataBehaviour missingData, Func<T1, T2, S> joinBehaviour, S defaultValue = default)
        {
            MissingData = missingData;
            JoinBehaviour = joinBehaviour ?? throw new ArgumentNullException(nameof(joinBehaviour));
            DefaultValue = defaultValue;
        }

        public S GetValue(DateTime date, IDateSequence<T1> t1, IDateSequence<T2> t2)
        {
            if (t1.ContainsKey(date) && t2.ContainsKey(date))
            {
                return JoinBehaviour(t1[date], t2[date]);
            }

            switch (MissingData)
            {
                case MissingDataBehaviour.DefaultValue:
                    return DefaultValue;

                case MissingDataBehaviour.ThrowException:
                    throw new KeyNotFoundException($"No data on {date:dd/MM/yyyy}");

                case MissingDataBehaviour.BackToLast:
                    IEnumerable<DateTime> backDates = t1.Keys.Intersect(t2.Keys).Where(x => x < date);
                    if (!backDates.Any())
                    {
                        return DefaultValue;
                    }
                    date = backDates.Max();
                    return JoinBehaviour(t1[date], t2[date]);

                case MissingDataBehaviour.ForwardToNext:
                    IEnumerable<DateTime> forwardDates = t1.Keys.Intersect(t2.Keys).Where(x => x > date);
                    if (!forwardDates.Any())
                    {
                        return DefaultValue;
                    }
                    date = forwardDates.Min();
                    return JoinBehaviour(t1[date], t2[date]);

                default:
                    throw new NotImplementedException($"No data on {date:dd/MM/yyyy} and no implementation for {MissingData.ToLongString()}");
            }
        }
    }
}
