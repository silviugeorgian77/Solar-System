using System.Collections.Generic;
using UnityEngine;

public class RandomUtils
{
    public static T Pick<T>(IList<T> entries)
    {
        if (entries.Count == 0)
        {
            return default;
        }

        var randomIndex = Random.Range(0, entries.Count);
        return entries[randomIndex];
    }

    public static T Pick<T>(IList<Pair<T, float>> entries)
    {
        var currentValue = 0f;
        List<RandomEntry<T>> randomEntries = new();
        RandomEntry<T> randomEntry;
        foreach (var entry in entries)
        {
            randomEntry = new();
            randomEntry.t = entry.Element1;
            randomEntry.min = currentValue;

            currentValue += entry.Element2;

            randomEntry.max = currentValue;

            randomEntries.Add(randomEntry);
        }

        var randomIndex = Random.Range(0, currentValue);
        foreach (var entry in randomEntries)
        {
            if (randomIndex >= entry.min && randomIndex <= entry.max)
            {
                return entry.t;
            }
        }

        return default;
    }

    public class RandomEntry<T>
    {
        public T t;
        public float min;
        public float max;
    }

    public static List<int> GenerateNumbersThatAddUpToGivenSum(
        int count,
        int total,
        int lowerBound,
        int upperBound)
    {

        List<int> result = new();
        int currentsum = 0;
        int low, high, calc;

        if ((upperBound * count) < total ||
            (lowerBound * count) > total ||
            upperBound < lowerBound)
            throw new System.Exception("Not possible.");

        for (int index = 0; index < count; index++)
        {
            calc = (total - currentsum) - (upperBound * (count - 1 - index));
            low = calc < lowerBound ? lowerBound : calc;
            calc = (total - currentsum) - (lowerBound * (count - 1 - index));
            high = calc > upperBound ? upperBound : calc;

            result.Add(Random.Range(low, high + 1));

            currentsum += result[index];
        }

        // The tail numbers will tend to drift higher or lower so we should
        // shuffle to compensate.
        result.Shuffle();

        return result;
    }

}
