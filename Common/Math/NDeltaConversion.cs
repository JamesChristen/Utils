namespace Common.Math
{
    public class NDeltaConversion
    {
        public static double NToDelta(int n)
        {
            return (double)(n - 1) / (n + 1);
        }

        public static double DeltaToN(double delta)
        {
            return (delta + 1) / (1 - delta);
        }
    }
}
