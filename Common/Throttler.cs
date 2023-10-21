using Common.Validation;
using System.Collections.Concurrent;

namespace Common
{
    public static class Throttler
    {
        public static async Task RunInParallel<T>(IEnumerable<T> items, Func<T, Task> func, int maxThreads = 4)
        {
            Validate.That
                    .IsPositive(maxThreads, nameof(maxThreads))
                    .IsNotNull(items, nameof(items))
                    .IsNotNull(func, nameof(func))
                    .Check();

            ConcurrentQueue<T> q = new ConcurrentQueue<T>(items);
            ConcurrentBag<Exception> exceptions = new ConcurrentBag<Exception>();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < maxThreads; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (q.TryDequeue(out T t))
                    {
                        try
                        {
                            await func(t);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(new Exception($"Error running task for {t}: {ex.Message}", ex));
                            continue;
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);
            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public static async Task<List<S>> RunInParallel<T, S>(IEnumerable<T> items, Func<T, Task<S>> func, int maxThreads = 4)
        {
            return await RunInParallel(items, func, (t, s) => s, maxThreads);
        }

        public static async Task<List<R>> RunInParallel<T, S, R>(IEnumerable<T> items, Func<T, Task<S>> func, Func<T, S, R> converter, int maxThreads = 4)
        {
            Validate.That
                    .IsPositive(maxThreads, nameof(maxThreads))
                    .IsNotNull(items, nameof(items))
                    .IsNotNull(func, nameof(func))
                    .IsNotNull(converter, nameof(converter))
                    .Check();

            ConcurrentQueue<T> q = new ConcurrentQueue<T>(items);
            ConcurrentBag<R> results = new ConcurrentBag<R>();
            ConcurrentBag<Exception> exceptions = new ConcurrentBag<Exception>();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < maxThreads; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (q.TryDequeue(out T t))
                    {
                        S s;
                        try
                        {
                            s = await func(t);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(new Exception($"Error running task for {t}: {ex.Message}", ex));
                            continue;
                        }

                        try
                        {
                            R r = converter(t, s);
                            results.Add(r);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(new Exception($"Error converting result of task for {t}: {ex.Message}", ex));
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);
            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
            return results.ToList();
        }
    }
}
