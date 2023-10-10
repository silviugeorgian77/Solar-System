using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ReflectionUtils
{
    public static T GetFieldValue<T>(this object obj, string name)
    {
        var field = obj
            .GetType()
            .GetField(
                name,
                BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
            );
        return (T)field?.GetValue(obj);
    }
}
