﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.ExtensionMethods
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable) action(item);
        }
    }
}
