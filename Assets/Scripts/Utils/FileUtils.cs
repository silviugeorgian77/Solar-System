using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class FileUtils 
{
    /// <summary>
    /// Writes the given text to the file at the given path
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The first parameter is the path.
    /// The second parameter is the text.
    /// </param>
    public static async Task WriteAllText(
        string path,
        string text,
        Action<string, string> onWriteAllTextCompleted)
    {
        await Task.Run(() =>
        {
            try
            {
                string directoriesPath = path.Substring(
                    0,
                    path.LastIndexOf(Path.DirectorySeparatorChar)
                );
                Directory.CreateDirectory(directoriesPath);
                File.WriteAllText(path, text);
            }
            catch
            {
            }
        });
        onWriteAllTextCompleted?.Invoke(path, text);
    }

    /// <summary>
    /// Reads the text of to the file at the given path
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The first parameter is the path.
    /// The second parameter is the text.
    /// </param>
    public async static Task ReadAllText(
        string path,
        Action<string, string> onReadAllTextCompleted)
    {
        string text = null;
        await Task.Run(() =>
        {
            try
            {
                if (File.Exists(path))
                {
                    text = File.ReadAllText(path);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
        onReadAllTextCompleted?.Invoke(path, text);
    }

    /// <summary>
    /// Writes the given byte array to the file at the given path
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The first parameter is the path.
    /// The second parameter is the byte array.
    /// </param>
    public async static Task WriteAllBytes(
       string path,
       byte[] bytes,
       Action<string, byte[]> onWriteAllBytesCompleted)
    {
        await Task.Run(() =>
        {
            try
            {
                string directoriesPath = path.Substring(
                    0,
                    path.LastIndexOf(Path.DirectorySeparatorChar)
                );
                Directory.CreateDirectory(directoriesPath);
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
        onWriteAllBytesCompleted?.Invoke(path, bytes);
    }

    /// <summary>
    /// Reads the byte array of to the file at the given path
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The first parameter is the path.
    /// The second parameter is the byte array.
    /// </param>
    public async static Task ReadAllBytes(
        string path,
        Action<string, byte[]> onReadAllBytesCompleted)
    {
        byte[] bytes = null;
        await Task.Run(() =>
        {
            try
            {
                if (File.Exists(path))
                {
                    bytes = File.ReadAllBytes(path);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
        onReadAllBytesCompleted?.Invoke(path, bytes);
    }

    /// <summary>
    /// Deletes the file at the given path
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The parameter is the path.
    /// </param>
    public async static Task DeleteFile(
        string path,
        Action<string> onFileDeleteCompleted)
    {
        await Task.Run(() =>
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
        onFileDeleteCompleted?.Invoke(path);
    }

    /// <summary>
    /// Deletes the files from the given list of paths.
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The parameter is the list of paths.
    /// </param>
    public async static Task DeleteAllFiles(
        List<string> paths,
        Action<List<string>> onAllFilesDeleteCompleted)
    {
        await Task.Run(() =>
        {
            try
            {
                foreach (string path in paths)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
        onAllFilesDeleteCompleted?.Invoke(paths);
    }

    /// <summary>
    /// Deletes the directory at the given path.
    /// </summary>
    /// <param name="onWriteAllTextCompleted">
    /// The parameter is the directory path.
    /// </param>
    public async static Task DeleteDirectory(
        string path,
        bool recursive,
        Action<string> onDirectoryDeleteCompleted)
    {
        await Task.Run(() =>
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, recursive);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
        onDirectoryDeleteCompleted?.Invoke(path);
    }

    public static long GetDirectorySize(string directoryPath)
    {
        return GetDirectorySize(new DirectoryInfo(directoryPath));
    }

    public static long GetDirectorySize(DirectoryInfo d)
    {
        long size = 0;
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            size += fi.Length;
        }
        // Add subdirectory sizes.
        DirectoryInfo[] dis = d.GetDirectories();
        foreach (DirectoryInfo di in dis)
        {
            size += GetDirectorySize(di);
        }
        return size;
    }

    public static List<string> GetAllFilesInDirectory(
        string directoryPath,
        bool recursive)
    {
        List<string> files = new List<string>();
        try
        {
            foreach (string f in Directory.GetFiles(directoryPath))
            {
                files.Add(f);
            }
            if (recursive)
            {
                foreach (string d in Directory.GetDirectories(directoryPath))
                {
                    files.AddRange(GetAllFilesInDirectory(d, recursive));
                }
            }
        }
        catch
        {
            
        }

        return files;
    }

    public static List<string> GetAllDirectoriesInDirectory(
        string directoryPath,
        bool recursive)
    {
        List<string> directories = new List<string>();
        try
        {
            foreach (string d in Directory.GetDirectories(directoryPath))
            {
                directories.Add(d);
                if (recursive)
                {
                    directories.AddRange(
                        GetAllDirectoriesInDirectory(d, recursive)
                    );
                }
            }
        }
        catch
        {

        }

        return directories;
    }

    public static string CorrectFileName(string fileName)
    {
        char[] ILLEGAL_FILE_NAME_CHARACTERS = new char[] {
            '/',
            '\n',
            '\r',
            '\t',
            '\0',
            '\f',
            '`',
            '?',
            '*',
            '\\',
            '<',
            '>',
            '|',
            '\"',
            ':'
        };
        char letter;
        char invalidLetter;
        bool validLetter;
        string correctFileName = "";
        for (int i = 0; i < fileName.Length; i++)
        {
            letter = fileName[i];
            validLetter = true;
            for (int j = 0; j < ILLEGAL_FILE_NAME_CHARACTERS.Length; j++)
            {
                invalidLetter = ILLEGAL_FILE_NAME_CHARACTERS[j];
                if (letter == invalidLetter)
                {
                    validLetter = false;
                    break;
                }
            }
            if (validLetter)
            {
                correctFileName += letter;
            }
        }
        return correctFileName;
    }

    public class CreationTimeComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return x.CreationTime.CompareTo(y.CreationTime);
        }
    }
}
