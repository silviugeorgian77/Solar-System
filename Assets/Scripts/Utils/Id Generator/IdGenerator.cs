using System;
using System.Net.NetworkInformation;

public class IdGenerator
{
    private static long currentCounter = 0;
    private static long macAddressHashCode;

    static IdGenerator() {
        string macAddress = GetMacAddress();
        macAddressHashCode = macAddress.GetHashCode();
    }

    public static long GenerateUniqueId()
    {
        long id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            + macAddressHashCode
            + currentCounter;
        currentCounter++;
        return id;
    }

    public static string GetMacAddress()
    {
        string sMacAddress = null;
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in nics)
        { 
            sMacAddress = adapter.GetPhysicalAddress().ToString();
            if (sMacAddress != null)
            {
                break;
            }
        }
        return sMacAddress;
    }
}
