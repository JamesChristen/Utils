namespace Ardalis.GuardClauses
{
    public static class GuardExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static T NotNull<T>(this IGuardClause clause, T input, string parameterName)
        {
            if (input != null)
            {
                throw new ArgumentException($"Required input {parameterName} has to be null. (Parameter '{parameterName}')");
            }
            return input;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static T DoesNotContainKey<T, S>(this IGuardClause clause, Dictionary<T, S> dictionary, T requiredKey, string parameterName)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            if (!dictionary.ContainsKey(requiredKey))
            {
                throw new ArgumentException($"Dictionary does not contain key {requiredKey}. (Parameter '{parameterName}')");
            }
            return requiredKey;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static T InvalidInput<T>(this IGuardClause guardClause, T input, string parameterName, Func<T, bool> predicate, string message)
        {
            if (!predicate(input))
            {
                throw new ArgumentException(message);
            }
            return input;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static T InvalidInput<T>(this IGuardClause guardClause, T input, Func<T, bool> predicate, string message)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            bool result;

            try
            {
                result = predicate(input);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error evaluating predicate: {ex.Message}", ex);
            }

            if (!result)
            {
                throw new ArgumentException(message);
            }
            return input;
        }
    }
}
