using System;
using System.Collections.Generic;

namespace Pairs.Core.Extensions
{
    static class ListExtensions
    {
        internal static List<T> DoubleValues<T>(this List<T> list)
        {
            var ret = new List<T>(list.Count * 2);
            foreach (var item in list)
            {
                ret.Add(item);
                ret.Add(item);
            }
            return ret;
        }

        internal static List<T> Shuffle<T>(this List<T> list)
        {
            var ret = new List<T>(list);
            var rand = new Random();
            for (int i = 0; i < ret.Count; i++)
            {
                int j = rand.Next(ret.Count);
                T temp = ret[i];
                ret[i] = ret[j];
                ret[j] = temp;
            }
            return ret;
        }
    }
}
