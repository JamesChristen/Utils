namespace System
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Returns true if <paramref name="input"/> is within <paramref name="margin"/> of decimal
        /// </summary>
        /// <returns></returns>
        public static bool EqualsWithError(this decimal dec, decimal input, decimal margin = 1e-10M)
        {
            if (margin < 0M)
            {
                throw new ArgumentException($"{nameof(Decimal)}.{nameof(EqualsWithError)} {margin} cannot be less than zero");
            }

            return Math.Abs(dec - input) <= margin;
        }
    }
}
