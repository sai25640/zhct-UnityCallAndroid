using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using System;

public class SysVideoCamera : MonoBehaviour {

    AndroidJavaObject currentActivity;
    string filePath;
    byte[] bytes;  //数据大小
    string url = "http://192.168.1.125:8080/upload.php";
    List<FileInformation> videoList = new List<FileInformation>();
    public Transform videoDataContent;
    public GameObject mainPanel;
    public GameObject isUpLoadPanel;
    public GameObject upLoadPanel;
    public Text filePathText;
    public Image img;
   
    void Start()
    {
//#if UNITY_ANDROID
//        filePath = "/sdcard/DCIM/Camera/IMG_20171110_204607.jpg";//IMG_20171110_204607.jpg
//#endif

//#if UNITY_EDITOR
//        filePath = Application.dataPath + "/Textures/IMG_20171110_204607.jpg"; //IMG_20171110_204607.jpg
//#endif

        filePathText.text = filePath;
        currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
      
    }

    public void OnVideoButtonClick()
    {
        currentActivity.Call("ShowSysVideoCamera");
        Debug.Log("ShowSysVideoCamera"); 
    }

    public void OnShowUpLoadPanelButtonClick()
    {
        ClearList();
        InitVideoList();
        CreateVideoData();
        upLoadPanel.SetActive(true);
        mainPanel.SetActive(false);
        isUpLoadPanel.SetActive(false);

    }

    public void InitVideoList()
    {  
        List<FileInformation> list =DirectoryAllFiles.GetAllFiles(new DirectoryInfo(@"/sdcard/DCIM/Camera")); ////C:\Users\hp\Desktop\Debug
        var query = from file in list
                    where file.FileName.ToLower().Contains("vid") ////qq
                    select file;
        foreach (var file in query)
        {
            Debug.Log(string.Format("文件名：{0}---文件目录{1}", file.FileName, file.FilePath));
            filePathText.text = "";
            filePathText.text += file.FileName + "\r\n";
            videoList.Add(file);
        }
    }

    public void CreateVideoData()
    {
        for (int i = 0; i < videoList.Count; i++)
        {
            GameObject go = Instantiate(Resources.Load("VideoData")) as GameObject;
            go.transform.GetComponentInChildren<Text>().text = videoList[i].FileName;
            go.transform.parent = videoDataContent;
            VideoData vd = go.GetComponent<VideoData>();
            vd.FileName = videoList[i].FileName;
            vd.FilePath = videoList[i].FilePath;

            EventTriggerListener.Get(go).onClick += OnVideoDataButtonClick;
        }
    }

    public void OnVideoDataButtonClick(GameObject go)
    {
        string uri = go.GetComponent<VideoData>().FilePath;
        //通过uri显示是否上传
        ShowUpLoadView(uri);
    }

    public void ShowUpLoadView(string uri)
    {
        SetDataURI(uri);
        mainPanel.SetActive(false);
        upLoadPanel.SetActive(false);
        isUpLoadPanel.SetActive(true);
       
    }
    public void SetDataURI(string uri)
    {
        filePath = uri;
        Debug.Log("uri =" + uri);
        filePathText.text = "uri =" + uri;
    }
    public void OnYesButtonClick()
    {
        //通过uri检测文件大小是否在20M以内
        if (CheckDataSizeCanUpLoadByUri(filePath))
        {
            StartCoroutine("PostData");
        }
        else
        {
            ShowMessage();
        }     
        OnNoButtonClick();
       
    }

    public void ClearList()
    {
        //防止重复加载
        videoList.Clear();
        DirectoryAllFiles.ClearList();
        for (int i = 0; i < videoDataContent.childCount; i++)
        {
            Destroy(videoDataContent.GetChild(i).gameObject);
        }
    }

    public void OnNoButtonClick()
    {
        mainPanel.SetActive(true);
        isUpLoadPanel.SetActive(false);
    }


    IEnumerator PostData()
    {
        WWWForm form = new WWWForm();
        //form.AddField("Content-Type", "multipart/form-data");  
        form.AddBinaryData("myFile", bytes, filePath.Substring(filePath.LastIndexOf("/") + 1));
        using (var w = UnityWebRequest.Post(url, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
                filePathText.text = w.error;
            }
            else
            {
                print("Finished Uploading Screenshot");
                filePathText.text = "Finished Uploading Screenshot";

            }
        }
    }

    bool CheckDataSizeCanUpLoadByUri(string uri)
    {
        if (File.Exists(uri))
        {
            bytes = File.ReadAllBytes(uri);
            filePathText.text = "字节大小为：" + bytes.Length;
            if (bytes.Length >= 50 * 1024 * 1024) // 大于50M限制
            {
                return false;
            }
            else
                return true;
        }
        else
        {
            filePathText.text = "路径不存在";
        }
        return false;
    }

    IEnumerator UriToByte(string uri)
    {

        if (File.Exists(uri))
        {
            bytes = File.ReadAllBytes(uri);
        }
        else
        {
            filePathText.text = "路径不存在";
        }
        yield return new WaitForSeconds(0.01f);
    }

    IEnumerator ByteToImage()
    {
        //byte[] bytes = o.GetByteArray("b_1").Bytes;//资源
        int width = 1080;
        int height = 640;
        Texture2D texture = new Texture2D(width, height);
        byte[] bytes = File.ReadAllBytes(filePath);
        texture.LoadImage(bytes);
        yield return new WaitForSeconds(0.01f);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        img.sprite = sprite;

        yield return new WaitForSeconds(0.01f);
        Resources.UnloadUnusedAssets(); //一定要清理游离资源。
    }

    public void ShowMessage()
    {
        filePathText.text = "字节大小为：" + bytes.Length+"\r\n"+string.Format("视频录制时间超过10秒,请确保在10秒以内！"); 
    }
}
