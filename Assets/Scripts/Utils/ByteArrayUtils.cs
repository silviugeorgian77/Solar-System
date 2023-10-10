using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ByteArrayUtils : MonoBehaviour
{
    public static byte[] ObjectToByteArray<T>(T obj)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return default;
        }
    }

    public static T ByteArrayToObject<T>(byte[] arrBytes)
    {
        try
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return (T)binForm.Deserialize(memStream);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return default;
        }
    }
}
