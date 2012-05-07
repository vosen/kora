using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    internal static class ICollectionHelpers
    {
        public static void ThrowIfInsufficientArray<T>(ICollection<T> col, T[] arr, int index)
        {
            if (arr == null)
                throw new ArgumentNullException();

            if (index < 0)
                throw new ArgumentOutOfRangeException();

            if (col.Count > arr.Length - index)
                throw new ArgumentException();
        }
    }
}
