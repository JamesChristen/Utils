using Common.Validation;

namespace Common
{
    public static class Retry
    {
        public static T NTimes<T, TException>(int nTimes, Func<T> operation, TimeSpan? retryWait = null) where TException : Exception
        {
            Validate.That
                    .IsPositive(nTimes, nameof(nTimes))
                    .IsNotNull(operation, nameof(operation))
                    .Check();

            TException error = null;
            for (int i = 0; i < nTimes; i++)
            {
                try
                {
                    return operation();
                }
                catch (TException ex)
                {
                    error = ex;
                    Thread.Sleep(retryWait ?? TimeSpan.Zero);
                }
            }

            throw error;
        }

        public static T NTimes<T>(int nTimes, Func<T> operation, TimeSpan? retryWait = null)
            => NTimes<T, Exception>(nTimes, operation, retryWait);

        public static void NTimes<TException>(int nTimes, Action operation, TimeSpan? retryWait = null) where TException : Exception
        {
            Validate.That
                    .IsPositive(nTimes, nameof(nTimes))
                    .IsNotNull(operation, nameof(operation))
                    .Check();

            TException error = null;
            for (int i = 0; i < nTimes; i++)
            {
                try
                {
                    operation();
                    return;
                }
                catch (TException ex)
                {
                    error = ex;
                    Thread.Sleep(retryWait ?? TimeSpan.Zero);
                }
            }

            throw error;
        }

        public static void NTimes(int nTimes, Action operation, TimeSpan? retryWait = null)
            => NTimes<Exception>(nTimes, operation, retryWait);
    }
}
