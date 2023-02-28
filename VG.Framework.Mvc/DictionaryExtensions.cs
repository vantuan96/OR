using System.Collections.Generic;

namespace VG.Framework.Mvc
{
    public static class DictionaryExtensions
    {
        public static void MergeKey<TKey, TValue>(this IDictionary<TKey, TValue> first, TKey key, TValue value, bool IsOveride = false)
        {
            if (key == null) return;
            if (first == null) first = new Dictionary<TKey, TValue>();

            if (!first.ContainsKey(key))
                first.Add(key, value);
            else
            {
                if (IsOveride)
                {
                    first.MergeKey(key, value);
                }
            }
        }

        public static void MergeDictionary<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second, bool IsOveride = false)
        {
            if (second == null) return;
            if (first == null) first = new Dictionary<TKey, TValue>();
            foreach (var item in second)
            {
                if (!first.ContainsKey(item.Key))
                    first.Add(item.Key, item.Value);
                else
                {
                    if (IsOveride)
                    {
                        first.MergeKey(item.Key, item.Value);
                    }
                }
            }
        }
    }
}
