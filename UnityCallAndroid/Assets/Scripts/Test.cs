using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class Test : MonoBehaviour {

    void Start()
    {
        List<FileInformation> list = DirectoryAllFiles.GetAllFiles(new DirectoryInfo(@"C:\Users\hp\Desktop\Debug"));
        var query = from file in list
                            where file.FileName.ToLower().Contains("qq")
                             select file;
        foreach (var file in query)
        {
            Debug.Log(string.Format("文件名：{0}---文件目录{1}", file.FileName, file.FilePath));
        }
        //if (list.Where(t => t.FileName.ToLower().Contains("qq")).Any())
        //{
        //    Debug.Log("true");
        //}
        //foreach (var item in list)
        //{
        //    Debug.Log(string.Format("文件名：{0}---文件目录{1}", item.FileName, item.FilePath));
        //}
    }


}
