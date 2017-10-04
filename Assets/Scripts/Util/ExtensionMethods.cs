using System;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    public static class ExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable) action(item);
        }
    }
}
