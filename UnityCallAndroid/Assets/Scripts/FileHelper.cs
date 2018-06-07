using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DirectoryAllFiles
{
    static List<FileInformation> FileList = new List<FileInformation>();
    public static List<FileInformation> GetAllFiles(DirectoryInfo dir)
    {
        FileInfo[] allFile = dir.GetFiles();
        foreach (FileInfo fi in allFile)
        {
            FileList.Add(new FileInformation { FileName = fi.Name, FilePath = fi.FullName });
        }
        DirectoryInfo[] allDir = dir.GetDirectories();
        foreach (DirectoryInfo d in allDir)
        {
            GetAllFiles(d);
        }
        return FileList;
    }

    public static void ClearList()
    {
        FileList.Clear();
    }
}

public class FileInformation
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
}
