using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using SimpleJson;
using System.Collections.Generic;

public class ExportAB {

	[MenuItem("Custom Editor/Build Windows AssetBundles")]
	static void CreateWindowsAssetBundles(){
		//BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets");

		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
	}

	[MenuItem("Custom Editor/Build IOS AssetBundles")]
	static void CreateiOSAssetBundlesMain(){
		//BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets");

		BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
	}

    [MenuItem("Custom Editor/Build Android AssetBundles")]
    static void CreateAndroidAssetBundles()
    {
        //BuildPipeline.BuildAssetBundles ("Assets/StreamingAssets");

        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }

    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();
    static void BuildFileIndex()
    {
        string resPath = Application.dataPath.ToLower() +"/StreamingAssets/";
        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/" + DownloadManager.MD5_FILE_NAME;


        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear(); files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        JsonObject dic = new JsonObject();
        dic["force"] = 110;
        dic["time"] = DateTime.Now.ToString("yyyyMMddhhmmss");
        dic["svn"] = 1;
        JsonObject fileDic = new JsonObject();
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = FileUtil.Instance.Md5file(file);
            FileInfo fileInfo = new FileInfo(file);
            string value = file.Replace(resPath, string.Empty);


            JsonObject fileData = new JsonObject();
            fileData["md5"] = md5;
            fileData["size"] = file.Length;
            fileDic[value] = fileData;
        }
        dic["files"] = fileDic;
        sw.Write(dic.ToString());
        sw.Close(); fs.Close();

        string hashPath = resPath + "/" + DownloadManager.MD5_HASH_NAME;
        fs = new FileStream(hashPath, FileMode.CreateNew);
        sw = new StreamWriter(fs);
        sw.Write(FileUtil.Instance.Md5file(newFilePath));
        sw.Close(); fs.Close();
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }
}