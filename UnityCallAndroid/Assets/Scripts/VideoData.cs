using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoData : MonoBehaviour {

    private string fileName;
    private string filePath;

    public string FileName
    {
        get
        {
            return fileName;
        }

        set
        {
            fileName = value;
        }
    }
    public string FilePath
    {
        get
        {
            return filePath;
        }

        set
        {
            filePath = value;
        }
    }

    public VideoData(string name,string path)
    {
        this.fileName = name;
        this.filePath = path;
    }
}
