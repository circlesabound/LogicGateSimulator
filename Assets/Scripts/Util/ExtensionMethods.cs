﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class ExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable) action(item);
        }

        public static GameObject FindChildGameObject(this GameObject go, string path)
        {
            var t = go.transform.Find(path);
            return t?.gameObject;
        }
    }
}
