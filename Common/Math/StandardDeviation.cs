namespace Common.Math
{
    public static class StandardDeviation
    {
        public static decimal Calculate(IEnumerable<decimal> values)
        {
            double stdDev = 0d;
            decimal[] arr = values?.ToArray() ?? Array.Empty<decimal>();
            int len = arr.Length;
            if (len > 1)
            {
                decimal avg = Mean.Calculate(arr);
                decimal sum = arr.Sum(x => (x - avg) * (x - avg));
                stdDev = System.Math.Sqrt((double)sum / len);
            }
            return Convert.ToDecimal(stdDev);
        }
    }
}
