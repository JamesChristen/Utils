using System.Linq;

namespace System.Collections.Generic
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns a one dimensional array. [ [1, 2, 3], [4, 5, 6] ] becomes [1, 2, 3, 4, 5, 6 ]
        /// </summary>
        public static T[] Flatten<T>(this T[,] array)
        {
            ArgumentNullException.ThrowIfNull(array);

            T[] flat = new T[array.GetLength(0) * array.GetLength(1)];
            int iLen = array.GetLength(0);
            int jLen = array.GetLength(1);
            for (int i = 0; i < iLen; i++)
            {
                for (int j = 0; j < jLen; j++)
                {
                    flat[(i * jLen) + j] = array[i, j];
                }
            }
            return flat;
        }

        /// <summary>
        /// Returns the specified column. Throws IndexOutOfRangeException if <paramref name="columnNumber"/> is out of range
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T[] GetColumn<T>(this T[,] array, int columnNumber)
        {
            ArgumentNullException.ThrowIfNull(array);

            int colLength = array.GetLength(0);
            if (columnNumber < 0 || columnNumber > colLength)
            {
                throw new IndexOutOfRangeException($"{nameof(columnNumber)} out of range of array ({typeof(T).Name}[{colLength},{array.GetLength(1)}]");
            }

            return Enumerable.Range(0, array.GetLength(0))
                    .Select(x => array[x, columnNumber])
                    .ToArray();
        }

        /// <summary>
        /// Returns the specified row. Throws IndexOutOfRangeException if <paramref name="rowNumber"/> is out of range
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T[] GetRow<T>(this T[,] array, int rowNumber)
        {
            ArgumentNullException.ThrowIfNull(array);

            int rowLength = array.GetLength(0);
            if (rowNumber < 0 || rowNumber > rowLength)
            {
                throw new IndexOutOfRangeException($"{nameof(rowNumber)} out of range of array ({typeof(T).Name}[{array.GetLength(0)},{rowLength}]");
            }

            return Enumerable.Range(0, array.GetLength(1))
                    .Select(x => array[rowNumber, x])
                    .ToArray();
        }
    }
}
