using System.Globalization;

public static class StringUtils
{
    public static string CapitalizeAllWordsBeforeSpace(
        this string currentString)
    {
        if (currentString.Length == 0)
        {
            return currentString;
        }
        string[] words = currentString.Split(' ');
        string result = "";
        foreach (string word in words)
        {
            result += CapitalizeFirstLetter(word) + " ";
        }
        return result.Substring(0, result.Length - 1);
    }

    public static string CapitalizeFirstLetter(
        this string currentString)
    {
        if (currentString.Length == 0)
        {
            return currentString;
        }
        return char.ToUpper(currentString[0])
            + currentString.Substring(1).ToLower();
    }

    public static string ReplaceFirst(
        this string text,
        string search,
        string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos)
            + replace
            + text.Substring(pos + search.Length);
    }

    public static string ReplaceAt(
        this string input,
        int index,
        char newChar)
    {
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }

    public static bool IsEmpty(this string text)
    {
        return text == null || text.Length == 0;
    }

    public static bool AreEqualInvarianCulture(
        this string str1,
        string str2)
    {
        return
            string.Compare(
                str1,
                str2,
                CultureInfo.InvariantCulture,
                CompareOptions.IgnoreNonSpace
            )
            == 0;
    }

    public static bool ContainsInvariantCulture(
        this string source,
        string toCheck)
    {
        return CultureInfo
            .InvariantCulture
            .CompareInfo
            .IndexOf(source, toCheck, CompareOptions.IgnoreNonSpace)
            != -1;
    }
}
