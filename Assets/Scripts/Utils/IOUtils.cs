using System.IO;

public class IOUtils
{
    public static string GetParentFolderPathFromFolderPath(string folderPath)
    {
        return folderPath.Substring(
            0,
            folderPath.LastIndexOf(Path.DirectorySeparatorChar)
        );
    }

    public static string GetFolderNameFromFolderPath(string folderPath)
    {
        int index = folderPath.LastIndexOf(Path.DirectorySeparatorChar) + 1;
        return folderPath.Substring(index, folderPath.Length - index);
    }
}
