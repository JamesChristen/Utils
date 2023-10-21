namespace System
{
    public static class NullHelper
    {
        public static T ThrowIfArgNull<T>(T input, string paramName)
        {
            ArgumentNullException.ThrowIfNull(input, paramName);
            return input;
        }
    }
}
