namespace Common.Math
{
    public static class Mean
    {
        public static decimal Calculate(IEnumerable<decimal> values)
        {
            decimal[] arr = values?.ToArray() ?? Array.Empty<decimal>();
            return arr.Length == 0 ? 0M : arr.Average();
        }
    }
}
