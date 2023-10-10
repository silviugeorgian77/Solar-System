using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class JsonAsync
{
    public async static Task ToJson(
        object obj,
        Action<string> onToJsonCompleted,
        bool prettyPrint = true)
    {
        string json = null;
        await Task.Run(() =>
        {
            json = JsonConvert.SerializeObject(
                obj,
                prettyPrint ? Formatting.Indented : Formatting.None
            );
        });
        onToJsonCompleted?.Invoke(json);
    }

    public async static Task FromJson<T>(
        string json,
        Action<T> onFromJsonCompleted)
    {
        if (json == null)
        {
            json = string.Empty;
        }
        T t = default;
        await Task.Run(() =>
        {
            t = JsonConvert.DeserializeObject<T>(json);
        });
        onFromJsonCompleted?.Invoke(t);
    }
}
