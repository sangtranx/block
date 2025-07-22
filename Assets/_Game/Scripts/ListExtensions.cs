using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Swap<T>(this IList<T> lst, int index1, int index2)
    {
        var tmp = lst[index1];
        lst[index1] = lst[index2];
        lst[index2] = tmp;
    }
    public static void Swap<T>(this IList<T> lst, T value1, T value2)
    {
        int index1 = lst.IndexOf(value1);
        int index2 = lst.IndexOf(value2);
        if (index1 != -1 && index2 != -1)
        {
            lst.Swap(index1, index2);
        }
    }
    public static List<int> FindAllIndex<T>(this IList<T> lst, Func<T, bool> condition)
    {
        List<int> lstIndex = new List<int>();
        for (int i = 0; i < lst.Count; i++)
        {
            if (condition != null && condition.Invoke(lst[i]))
            {
                lstIndex.Add(i);
            }
        }
        return lstIndex;
    }
    public static int CountElementTrueFor<T>(this IList<T> list, Predicate<T> condition)
    {
        int count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (condition != null && condition.Invoke(list[i]))
            {
                count++;
            }
        }
        return count;
    }
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
