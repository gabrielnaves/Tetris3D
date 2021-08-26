using System;
using System.Collections.Generic;

static public class ListExtensions {

    /// From https://stackoverflow.com/questions/273313/randomize-a-listt
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool IsEqualTo<T>(this IList<T> list, IList<T> other) {
        if (list.Count != other.Count)
            return false;
        for (int i = 0; i < list.Count; ++i) {
            if (!list[i].Equals(other[i]))
                return false;
        }
        return true;
    }

    public static void RemoveLast<T>(this IList<T> list) {
        list.RemoveAt(list.Count - 1);
    }

    public static void Resize<T>(this IList<T> list, int length) {
        while (list.Count > length)
            list.RemoveLast();
        while (list.Count < length)
            list.Add(list[list.Count - 1]);
    }

    public static int ReduceToInt<T>(this IList<T> list, Func<T, int> op, int start) {
        foreach (T item in list)
            start += op(item);
        return start;
    }
}
