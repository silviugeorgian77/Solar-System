using System;
using System.Threading.Tasks;

public class JsonSaver
{
    public static async Task Save<T>(
        T t,
        string path,
        Action<T> onDataSaved)
    {
        string jsonResult = null;
        await JsonAsync.ToJson(t, (json) =>
        {
            jsonResult = json;
        });
        await FileUtils.WriteAllText(path, jsonResult, null);
        onDataSaved?.Invoke(t);
    }

    public static async Task Load<T>(
        string path,
        Action<T> onDataLoaded)
    {
        string jsonResult = null;
        await FileUtils.ReadAllText(path, (path, json) =>
        {
            jsonResult = json;
        });
        T tResult = default;
        await JsonAsync.FromJson<T>(jsonResult, (t) =>
        {
            tResult = t;
        });
        onDataLoaded?.Invoke(tResult);
    }
}
