
using System;
using System.Linq;

public class ArrayUtils
{
    public static T[] Combine<T>(params T[][] arrays)
    {
        T[] rv = new T[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach (T[] array in arrays)
        {
            Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }

    public static T[] SubArray<T>(T[] data, int index, int length)
    {
        T[] result = new T[length];
        Buffer.BlockCopy(data, index, result, 0, length);
        return result;
    }
}
