namespace Common.Sequences
{
    public class SequenceOptions
    {
        public bool AutoFillDates { get; set; } = false;
        public bool IncludeWeekends { get; set; } = false;
        public SequenceFrequency Frequency { get; set; } = SequenceFrequency.Daily;
        public MissingDataBehaviour MissingData { get; set; } = MissingDataBehaviour.DefaultValue;

        public TimeSpan BarTime { get; set; }
        public TimeSpan BarStart { get; set; }
        public TimeSpan BarEnd { get; set; }

        public SequenceOptions Clone()
        {
            return new SequenceOptions
            {
                AutoFillDates = AutoFillDates,
                IncludeWeekends = IncludeWeekends,
                Frequency = Frequency,
                BarTime = BarTime,
                BarStart = BarStart,
                BarEnd = BarEnd
            };
        }

        public override bool Equals(object obj)
        {
            return obj is SequenceOptions options &&
                   AutoFillDates == options.AutoFillDates &&
                   IncludeWeekends == options.IncludeWeekends &&
                   Frequency == options.Frequency &&
                   BarTime.Equals(options.BarTime) &&
                   BarStart.Equals(options.BarStart) &&
                   BarEnd.Equals(options.BarEnd);
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(AutoFillDates);
            hash.Add(IncludeWeekends);
            hash.Add(Frequency);
            hash.Add(BarTime);
            hash.Add(BarStart);
            hash.Add(BarEnd);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            return $"Frequency: {Frequency} | AutoFill: {AutoFillDates} | IncludeWeekends: {IncludeWeekends}";
        }
    }
}
