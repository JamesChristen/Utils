namespace Common.Math
{
    public static class LinearInterpolation
    {
        public static decimal Calculate(decimal y1, decimal y2, DateTime d1, DateTime d2, DateTime d)
        {
            return Calculate(y1, y2, d1.Ticks, d2.Ticks, d.Ticks);
        }

        public static decimal Calculate(decimal y1, decimal y2, long x1, long x2, long x)
        {
            if (y1 == y2)
            {
                return y1;
            }
            if (x1 - x2 == 0)
            {
                return (y1 + y2) / 2;
            }
            return y1 + (x - x1) * (y2 - y1) / (x2 - x1);
        }
    }
}
