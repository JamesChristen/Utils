namespace Common.Sequences
{
    public class TimeSequence : DateSequence<decimal>, ITimeSequence
    {
        public TimeSequence()
        {
        }

        public TimeSequence(Dictionary<DateTime, decimal> items)
            : base(items)
        {
        }

        public TimeSequence(SequenceOptions options)
            : base(options)
        {
        }

        public TimeSequence(SequenceOptions options, Dictionary<DateTime, decimal> items)
            : base(options, items)
        {
        }

        public TimeSequence(IDateSequence<decimal> seq)
        {
            Options = seq.Options.Clone();
            _series = new SortedList<DateTime, decimal>(seq.ToDictionary(x => x.Key, x => x.Value));
            AutoFillIfNeeded();
        }

        public static new ITimeSequence Empty => new TimeSequence();

        public static ITimeSequence Constant(DateTime? start, DateTime? end, decimal value)
        {
            if (start.HasValue && end.HasValue && start.Value <= end.Value)
            {
                DateInterval interval = new DateInterval(start.Value, end.Value);
                Dictionary<DateTime, decimal> values = interval.GetDates().ToDictionary(x => x, x => value);
                return new TimeSequence(values);
            }
            return Empty;
        }

        public new ITimeSequence InRange(DateTime startDate, DateTime endDate)
        {
            return new TimeSequence(
                Options.Clone(),
                _series.Where(x => x.Key >= startDate && x.Key <= endDate).ToDictionary(x => x.Key, x => x.Value));
        }

        public ITimeSequence Absolute()
        {
            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => System.Math.Abs(x.Value)));
        }

        public ITimeSequence Negate()
        {
            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => -x.Value));
        }

        public ITimeSequence Add(ITimeSequence seq)
        {
            ArgumentNullException.ThrowIfNull(seq);

            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => x.Value + seq.GetValueOrDefault(x.Key, 0M)));
        }

        public ITimeSequence Subtract(ITimeSequence seq)
        {
            ArgumentNullException.ThrowIfNull(seq);

            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => x.Value - seq.GetValueOrDefault(x.Key, 0M)));
        }

        public ITimeSequence DivideBy(decimal denominator)
        {
            if (denominator == 0M)
            {
                throw new DivideByZeroException($"{nameof(denominator)} cannot be 0");
            }

            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => x.Value / denominator));
        }

        public ITimeSequence DivideBy(ITimeSequence seq)
        {
            ArgumentNullException.ThrowIfNull(seq);

            HashSet<DateTime> keys = Keys.ToHashSet();
            HashSet<DateTime> seqKeys = seq.Keys.ToHashSet();

            HashSet<DateTime> missingDivisorDates = keys.Except(seqKeys).ToHashSet();
            if (missingDivisorDates.Any())
            {
                throw new ArgumentException($"Incompatible dates - cannot divide {nameof(ITimeSequence)}");
            }

            if (keys.Any(x => seq[x] == 0M))
            {
                throw new DivideByZeroException($"Divisor sequence has entries with value of 0 - cannot divide {nameof(ITimeSequence)}");
            }

            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => x.Value / seq[x.Key]));
        }

        public ITimeSequence MultiplyBy(decimal multiplier)
        {
            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => x.Value * multiplier));
        }

        public ITimeSequence MultiplyBy(ITimeSequence seq)
        {
            ArgumentNullException.ThrowIfNull(seq);

            HashSet<DateTime> keys = Keys.ToHashSet();
            HashSet<DateTime> seqKeys = seq.Keys.ToHashSet();

            if (!keys.All(x => seqKeys.Contains(x)))
            {
                throw new ArgumentException($"Incompatible dates - cannot divide {nameof(ITimeSequence)}");
            }

            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => x.Value * seq[x.Key]));
        }

        public ITimeSequence SquareRoot()
        {
            if (Values.Any(x => x < 0M))
            {
                throw new InvalidOperationException($"Sequence has negative values = cannot square root {nameof(ITimeSequence)}");
            }

            return new TimeSequence(
                Options.Clone(), 
                this.ToDictionary(x => x.Key, x => Convert.ToDecimal(System.Math.Sqrt(Convert.ToDouble(x.Value)))));
        }

        public ITimeSequence MeanExpanding()
        {
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            decimal sum = 0M;
            int count = 0;
            foreach (KeyValuePair<DateTime, decimal> item in this)
            {
                sum += item.Value;
                count++;
                result[item.Key] = sum / count;
            }
            return new TimeSequence(Options.Clone(), result);
        }

        public ITimeSequence MeanWindowed(WindowLength windowLength)
        {
            if (windowLength.IsInfinite)
            {
                return MeanExpanding();
            }

            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            decimal sum = 0M;
            int count = 0;
            decimal[] window = new decimal[windowLength.Length];
            KeyValuePair<DateTime, decimal>[] arr = this.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                int index = i % window.Length;
                if (count >= window.Length)
                {
                    sum -= window[index];
                }
                else
                {
                    count++;
                }
                window[index] = arr[i].Value;
                sum += window[index];
                result[arr[i].Key] = sum / count;
            }

            return new TimeSequence(Options.Clone(), result);
        }

        public ITimeSequence StandardDeviationExpanding()
        {
            ITimeSequence variance = VarianceExpanding();
            return variance.SquareRoot();
        }

        public ITimeSequence StandardDeviationWindowed(WindowLength windowLength)
        {
            ITimeSequence variance = VarianceWindowed(windowLength);
            return variance.SquareRoot();
        }

        public ITimeSequence StandardDeviationPopulationExpanding()
        {
            ITimeSequence variance = VariancePopulationExpanding();
            return variance.SquareRoot();
        }

        public ITimeSequence StandardDeviationPopulationWindowed(WindowLength windowLength)
        {
            ITimeSequence variance = VariancePopulationWindowed(windowLength);
            return variance.SquareRoot();
        }

        public ITimeSequence VarianceExpanding()
        {
            ITimeSequence mean = MeanExpanding();
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            decimal sumSquared = 0M;
            int count = 0;
            foreach (KeyValuePair<DateTime, decimal> item in this)
            {
                sumSquared += item.Value * item.Value;
                count++;
                if (count == 1) // Using sample variance - cannot divide by zero
                {
                    continue;
                }
                result[item.Key] = (sumSquared - (mean[item.Key] * mean[item.Key] * count)) / (count - 1);
            }
            return new TimeSequence(Options.Clone(), result);
        }

        public ITimeSequence VarianceWindowed(WindowLength windowLength)
        {
            if (windowLength.IsInfinite)
            {
                return VarianceExpanding();
            }

            ITimeSequence mean = MeanWindowed(windowLength);
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            decimal sumSquared = 0M;
            int count = 0;
            decimal[] window = new decimal[windowLength.Length];
            KeyValuePair<DateTime, decimal>[] arr = this.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                int index = i % window.Length;
                if (count >= window.Length)
                {
                    sumSquared -= window[index];
                }
                else
                {
                    count++;
                }
                window[index] = arr[i].Value * arr[i].Value;
                sumSquared += window[index];

                if (count == 1) // Using sample variance - cannot divide by zero
                {
                    continue;
                }
                result[arr[i].Key] = (sumSquared - (mean[arr[i].Key] * mean[arr[i].Key] * count)) / (count - 1);
            }

            return new TimeSequence(Options.Clone(), result);
        }

        public ITimeSequence VariancePopulationExpanding()
        {
            ITimeSequence mean = MeanExpanding();
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            decimal sumSquared = 0M;
            int count = 0;
            foreach (KeyValuePair<DateTime, decimal> item in this)
            {
                sumSquared += item.Value * item.Value;
                count++;
                if (count == 1) // Using sample variance - cannot divide by zero
                {
                    result[item.Key] = 0M;
                    continue;
                }
                result[item.Key] = (sumSquared / count) - (mean[item.Key] * mean[item.Key]);
            }
            return new TimeSequence(Options.Clone(), result);
        }

        public ITimeSequence VariancePopulationWindowed(WindowLength windowLength)
        {
            if (windowLength.IsInfinite)
            {
                return VariancePopulationExpanding();
            }

            ITimeSequence mean = MeanWindowed(windowLength);
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            decimal sumSquared = 0M;
            int count = 0;
            decimal[] window = new decimal[windowLength.Length];
            KeyValuePair<DateTime, decimal>[] arr = this.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                int index = i % window.Length;
                if (count >= window.Length)
                {
                    sumSquared -= window[index];
                }
                else
                {
                    count++;
                }
                window[index] = arr[i].Value * arr[i].Value;
                sumSquared += window[index];

                if (count == 1) // Using sample variance - cannot divide by zero
                {
                    result[arr[i].Key] = 0M;
                    continue;
                }
                result[arr[i].Key] = (sumSquared / count) - (mean[arr[i].Key] * mean[arr[i].Key]);
            }

            return new TimeSequence(Options.Clone(), result);
        }

        public ITimeSequence ZScoreExpanding()
        {
            ITimeSequence mean = MeanExpanding();
            ITimeSequence stdDev = StandardDeviationExpanding();
            return ZScore(mean, stdDev);
        }

        public ITimeSequence ZScoreWindowed(WindowLength windowLength)
        {
            ITimeSequence mean = MeanWindowed(windowLength);
            ITimeSequence stdDev = StandardDeviationWindowed(windowLength);
            return ZScore(mean, stdDev);
        }

        private ITimeSequence ZScore(ITimeSequence mean, ITimeSequence stdDev)
        {
            Dictionary<DateTime, decimal> result = new Dictionary<DateTime, decimal>();
            foreach (DateTime date in stdDev.Keys)
            {
                if (stdDev[date] != 0M)
                {
                    result[date] = (this[date] - mean[date]) / stdDev[date];
                }
            }
            return new TimeSequence(Options.Clone(), result);
        }

        public static ITimeSequence Mean(params ITimeSequence[] inputs)
        {
            ArgumentNullException.ThrowIfNull(inputs);

            ITimeSequence[] sequences = inputs.Where(x => x != null && !x.IsEmpty).ToArray();
            sequences.ForEach(x => x.Options.MissingData = MissingDataBehaviour.BackToLast);

            DateTime minDate = sequences.Min(x => x.FirstKey.Value);
            DateTime maxDate = sequences.Max(x => x.LastKey.Value);

            Dictionary<DateTime, decimal> values = new Dictionary<DateTime, decimal>();
            for (DateTime date = minDate; date <= maxDate; date = date.NextBusinessDay())
            {
                decimal sum = 0M;
                int count = 0;
                for (int i = 0; i < sequences.Length; i++)
                {
                    if (sequences[i].FirstKey.Value <= date)
                    {
                        sum += sequences[i][date];
                        count++;
                    }
                }
                values.Add(date, sum / count);
            }

            return new TimeSequence(values);
        }
    }
}
