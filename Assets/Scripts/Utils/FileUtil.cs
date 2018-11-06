using UnityEngine;
using System.IO;
using System.Text;
using System;


public class FileUtil {
	private static FileUtil s_fileUtil = null;

	public static FileUtil Instance{
		get{
			if (s_fileUtil == null) {
				s_fileUtil = new FileUtil ();
			}

			return s_fileUtil;
		}
	}


	public void write(string file, string data){
		Debug.Log ("*****************************" + file);
		StreamWriter sw;
		FileInfo info = new FileInfo (file);

		if (!info.Exists) {
			sw = info.CreateText ();
		} else {
			sw = info.AppendText ();
		}

		sw.WriteLine (data);

		sw.Close ();
		sw.Dispose ();
	}

	public void DeleteFile(string file){
		File.Delete (file);
	}

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public string Md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    /// <summary>
	/// 取得数据存放目录 C#模式路径
    /// </summary>
    public string StoragePath
    {
        get{
            string game = "UTank";
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/res/";
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return Application.dataPath.Substring(0, i + 1) + "res/";
            }
			return "c:/" + game + "/res/";
        }   
    }

    /// <summary>
    /// 渠道程序包内路径 C#模式路径
    /// </summary>
    public string AppPath
    {
        get{
			return Application.streamingAssetsPath + "/";
        }
    }


	private const string ANDROID_STREAMING_TAG = " jar:file:";
	/// <summary>
	/// WWW模式路径
	/// </summary>
	/// <returns>The UR.</returns>
	/// <param name="path">Path.</param>
	public string GetURL(string path){
		if (RuntimePlatform.Android == Application.platform && path.IndexOf (ANDROID_STREAMING_TAG) > -1) {
			return path;
		} else {
			return "file://" + path;
		}
	}
}
