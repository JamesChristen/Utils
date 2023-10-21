namespace Common.Math
{
    public static class ZScore
    {
        public static IEnumerable<decimal> Calculate(IEnumerable<decimal> sequence, IEnumerable<decimal> refSequence, DistributionType distributionType)
        {
            return Calculate(sequence.Select(x => decimal.ToDouble(x)), refSequence.Select(x => decimal.ToDouble(x)), distributionType).Select(x => (decimal)x);
        }

        public static decimal Calculate(
            decimal value,
            IEnumerable<decimal> refSequence,
            DistributionType distributionType)
        {
            double average = Convert.ToDouble(refSequence.Average());
            double stdev = Convert.ToDouble(StandardDeviation.Calculate(refSequence));

            switch (distributionType)
            {
                case DistributionType.LogNormal:
                    {
                        double averageLogNormal = System.Math.Exp(average + System.Math.Pow(stdev, 2) / 2);
                        double stdevLogNormal = System.Math.Sqrt(
                            (System.Math.Exp(System.Math.Pow(stdev, 2)) - 1) * System.Math.Exp(2 * average + System.Math.Pow(stdev, 2)));
                        average = averageLogNormal;
                        stdev = stdevLogNormal;
                        break;
                    }
            }

            if (decimal.ToDouble(value) - average == 0 || stdev == 0)
            {
                return 0;
            }

            return (decimal)((decimal.ToDouble(value) - average) / stdev);
        }

        public static decimal Calculate(
            decimal value,
            double average,
            double stdDev,
            DistributionType distributionType)
        {
            switch (distributionType)
            {
                case DistributionType.LogNormal:
                    {
                        double averageLogNormal = System.Math.Exp(average + System.Math.Pow(stdDev, 2) / 2);
                        double stdevLogNormal = System.Math.Sqrt(
                            (System.Math.Exp(System.Math.Pow(stdDev, 2)) - 1) * System.Math.Exp(2 * average + System.Math.Pow(stdDev, 2)));
                        average = averageLogNormal;
                        stdDev = stdevLogNormal;
                        break;
                    }
            }

            if (decimal.ToDouble(value) - average == 0 || stdDev == 0)
            {
                return 0;
            }

            return (decimal)((decimal.ToDouble(value) - average) / stdDev);
        }

        public static IEnumerable<double> Calculate(
            IEnumerable<double> sequence,
            IEnumerable<double> refSequence,
            DistributionType distributionType)
        {
            IEnumerable<double> zScores = new List<double>();

            if (sequence.Any())
            {
                double average = refSequence.Average();
                double sum = refSequence.Sum(d => System.Math.Pow(d - average, 2));
                double stdev = System.Math.Sqrt((sum) / refSequence.Count());

                switch (distributionType)
                {
                    case DistributionType.LogNormal:
                        {
                            double averageLogNormal = System.Math.Exp(average + System.Math.Pow(stdev, 2) / 2);
                            double stdevLogNormal = System.Math.Sqrt(
                                (System.Math.Exp(System.Math.Pow(stdev, 2)) - 1) * System.Math.Exp(2 * average + System.Math.Pow(stdev, 2)));
                            average = averageLogNormal;
                            stdev = stdevLogNormal;
                            break;
                        }
                }

                zScores = sequence.Select(x => (x - average) / stdev);
            }
            return zScores;
        }
    }
}
