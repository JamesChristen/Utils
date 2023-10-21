using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace System
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Returns generic IEnumerable with a single item. If source is null an empty collection is returned
        /// </summary>
        public static IEnumerable<T> AsSingleEnumerable<T>(this T t)
        {
            if (t == null)
            {
                yield break;
            }
            yield return t;
        }

        /// <summary>
        /// Performs the actino specified on every item in source, regardless of the null status of the item
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (source != null)
            {
                foreach (T t in source)
                {
                    action(t);
                }
            }
        }

        /// <summary>
        /// Chunks source into evenly sized collections of length up to the <paramref name="chunksize"/>
        /// <paramref name="chunksize"/> must be greater than 0
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<IEnumerable<T>> ChunkEvenly<T>(this IEnumerable<T> source, int chunkSize)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (chunkSize <= 0)
            {
                throw new ArgumentException($"Chunk size has to be greater than 0");
            }

            int numElements = source.Count();
            int numChunks = (int)Math.Ceiling((double)numElements / chunkSize);
            int count = 0;

            int currentChunkSize = (int)Math.Ceiling((double)numElements / numChunks);

            List<T> temp = new List<T>();
            foreach (T element in source)
            {
                if (count++ == currentChunkSize)
                {
                    yield return temp;
                    temp = new List<T>();
                    count = 1;
                    numElements -= currentChunkSize;
                    currentChunkSize = (int)Math.Ceiling((double)numElements / --numChunks);
                }
                temp.Add(element);
            }

            yield return temp;
        }

        /// <summary>
        /// Get all children recursively for parent-child structure of given root, including branches
        /// </summary>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> originalList, Func<T, IEnumerable<T>> elementSelector)
        {
            ArgumentNullException.ThrowIfNull(elementSelector);

            if (originalList != null)
            {
                foreach (T element in originalList.Where(element => elementSelector(element) != null))
                {
                    originalList = originalList.Concat(Flatten(elementSelector(element).ToList(), elementSelector)).ToList();
                }
            }
            else
            {
                return new List<T>();
            }

            return originalList;
        }

        /// <summary>
        /// Evaluates every <typeparamref name="T"/> of <paramref name="items"/>
        /// </summary>
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            List<T> results = new List<T>();
            await foreach (T item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                results.Add(item);
            }
            return results;
        }
    }
}
