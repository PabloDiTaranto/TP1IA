using System.Collections.Generic;
using System.Linq;
using System;

public static class DictionaryExtensions {
    public static bool In<T>(this T x, HashSet<T> set) {
        return set.Contains(x);
    }

    public static bool In<K, V>(this KeyValuePair<K, V> x, Dictionary<K, V> dict) {
        return dict.Contains(x);
    }

    public static void UpdateWith<K, V>(this Dictionary<K, V> a, Dictionary<K, V> b) {
        foreach (var kvp in b) {
            a[kvp.Key] = kvp.Value;
        }
    }

    public static bool In(this KeyValuePair<string, Func<object, bool>> kvp, Dictionary<string, object> dict)
    {
        if (!dict.ContainsKey(kvp.Key)) { return false; }

        return kvp.Value.Invoke(dict[kvp.Key]);
    }

    public static void UpdateWith(this Dictionary<string, object> updated, Dictionary<string, Action<object>> updater)
    {
        foreach (var kvp in updater)
        {
            kvp.Value.Invoke(updated[kvp.Key]);
        }
    }
}