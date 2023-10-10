
using Newtonsoft.Json;

public static class DeepCloneUtils
{
    public static T DeepClone<T>(this T obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.None);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
