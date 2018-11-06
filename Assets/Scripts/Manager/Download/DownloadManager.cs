using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SimpleJson;
using System.IO;
using LuaInterface;

public class DownloadManager : MonoBehaviour
{
    private List<string> _urlList;

    public List<string> urlList
    {
        get { return _urlList; }
    }
    private string _srcRoot;
    private string _desRoot;
    private int _workThreadCount;

    private DownloadTaskManager _taskManager;

    private bool _encryptMd5File = false;   //是否加密监测文件  todo 还没做加密

    private JsonObject _md5App; //程序目录下的md5校验列表
    private JsonObject _md5Storage; //存储目录下的md5校验列表
    private JsonObject _md5Remote; //服务器上的md5校验列表
    private string _md5RemoteHash;  //服务器上md5校验文件的md5

    private JsonObject _md5AppFiles;
    private JsonObject _md5StorageFiles;
    private JsonObject _md5RemoteFiles;

    public delegate void DownloadCallback(string msg);

    public LuaFunction OnNetworkError; //网络错误
    public LuaFunction OnDownloadFileError;    //某个文件下载错误
    public LuaFunction OnForceUpdate;  //需要强更
    public LuaFunction OnProgress;     //更新中回调更新进度
    public LuaFunction OnComplete;     //更新完毕
    public LuaFunction OnDownloadSize; //检测更新时，提示要下载的文件大小

    public static bool DOWNLOAD_REMOTE = true; //是否下载远程文件 
    public const string MD5_FILE_NAME = "files.txt";
    public const string MD5_HASH_NAME = "files.hash";
    public const string MD5_REMOTE_CACHE_NAME = "files_remote.txt";

    public static DownloadManager instance;


    private List<string> downloadFiles = new List<string>();

    public static void Log(object message)
    {
        Debug.Log(message);
    }

    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    public string GetFilePath(string filename)
    {
        string appValue = _GetMd5(_md5AppFiles, filename);
        string remoteValue = _GetMd5(_md5RemoteFiles, filename);
        if (remoteValue == null || appValue == remoteValue)
        {
            return FileUtil.Instance.AppPath + filename;
        }
        else {
            return FileUtil.Instance.StoragePath + filename;
        }
    }

    public string GetFileUrl(string filename)
    {
        string appValue = _GetMd5(_md5AppFiles, filename);
        string remoteValue = _GetMd5(_md5RemoteFiles, filename);
        if (remoteValue == null || appValue == remoteValue)
        {
            string path = FileUtil.Instance.AppPath + filename;
            if (Application.platform == RuntimePlatform.Android)
            {
                return path;
            }
            return "file:///" + path;      
        }
        else
        {
            return "file:///" + FileUtil.Instance.StoragePath + filename;
        } 
    }

    /// <summary>
    /// 初始化下载参数
    /// </summary>
    /// <param name="urlList">服务器更新地址列表</param>
    /// <param name="workThreadCount">使用几个线程下载</param>
    public void Init(List<string> urlList, int workThreadCount)
    {
        _urlList = urlList;

        _srcRoot = FileUtil.Instance.AppPath;
        _desRoot = FileUtil.Instance.StoragePath;
        _workThreadCount = workThreadCount;

        instance = this;
    }

    /// <summary>
    /// 读取本地md5文件
    /// 更新前应先执行此函数
    /// </summary>
    public void LoadLocalMd5()
    {
        //读取程序目录md5
        DownloadManager.Log("loadapp");
        string appFile = _srcRoot + MD5_FILE_NAME;
        _md5App = _LoadLocalMd5(appFile);
        _md5AppFiles = _md5App["files"] as JsonObject;

        //读取存储目录md5
        DownloadManager.Log("loadstorage");
        string desFile = _desRoot + MD5_FILE_NAME;

        if (File.Exists(desFile))
        {   //注意读取程序目录时，不能加这个判断，否则即时文件存在，也会返回false。但是读取存储目录，一定要加这个判断，不然文件不存在要等好久
            _md5Storage = _LoadLocalMd5(desFile);
        }
        if (_md5Storage == null || _md5Storage.Count == 0)
        {
            _md5Storage = new JsonObject();
            _md5Storage["files"] = new JsonObject();
        }
        _md5StorageFiles = _md5Storage["files"] as JsonObject;

        //读取保存的远端md5
        string remoteCacheFile = _desRoot + MD5_REMOTE_CACHE_NAME;
        if (File.Exists(remoteCacheFile))
        {   //注意读取程序目录时，不能加这个判断，否则即时文件存在，也会返回false。但是读取存储目录，一定要加这个判断，不然文件不存在要等好久
            _md5Remote = _LoadLocalMd5(remoteCacheFile);
        }
        if (_md5Remote == null || _md5Remote.Count == 0)
        {
            _md5Remote = new JsonObject();
            _md5Remote["files"] = new JsonObject();
        }
        _md5RemoteFiles = _md5Remote["files"] as JsonObject;
    }

    /// <summary>
    /// 连接服务器，检测更新
    /// 有可能有三种情况
    /// 1. 不需要更新，直接跳过更新步骤 (直接回调OnComplete)
    /// 2. 需要强更 (会回调OnForceUpdate)
    /// 3. 需要更新 (会回调OnDownloadSize，用户点确定后可以调用StartDownload执行更新)
    /// </summary>
    public void CheckUpdate()
    {
        if (!DOWNLOAD_REMOTE)
        {
            _OnComplete();
            return;
        }
        this.StartCoroutine(_LoadRemoteMd5());
    }

    /// <summary>
    /// 执行更新 
    /// </summary>
    public void StartDownload()
    {
        this.StartCoroutine(_Download());
    }

    private JsonObject _LoadLocalMd5(string path)
    {
        //用www读取巨慢，改用StreamReader读取
        StreamReader sr = null;
        JsonObject md5 = null;
        try
        {
            sr = File.OpenText(path);
            string content = sr.ReadToEnd();

            md5 = new JsonObject();
            _ParseMd5File(ref md5, content);
        }
        catch (Exception) { }
        finally
        {
            if (sr != null)
            {
                sr.Close();
                sr.Dispose();
            }
        }

        return md5;
    }

    private IEnumerator _LoadRemoteMd5()
    {
        DownloadManager.Log("loadremote hash");
        //读取远端md5
        bool downloadSucc = false;
        WWW www = null;
        foreach (string url in _urlList)
        {
            DownloadManager.Log(url + MD5_HASH_NAME);
            www = new WWW(url + MD5_HASH_NAME);
            yield return www;

            if (www.error == null)
            {
                _md5RemoteHash = www.text;
                downloadSucc = true;
                break;
            }
            else
            {
                DownloadManager.LogWarning("loadlistfile hash error " + url + MD5_HASH_NAME);
            }
        }
        if (!downloadSucc)
        {
            //获取远程md5 hash读取错误时，报错
            _OnNetworkError(www.error);
            yield break;
        }

        if (_md5RemoteHash != null && _md5RemoteHash.Equals(_md5Remote["hash"] as string))
        {
            //优化，远端md5校验文件的hash，和本地的相同，则不需要再从远端下载了
            _OnComplete();
            yield break;
        }

        DownloadManager.Log("loadremote");
        downloadSucc = false;
        www = null;
        foreach (string url in _urlList) {
            DownloadManager.Log(url + MD5_FILE_NAME);
            www = new WWW(url + MD5_FILE_NAME);
            yield return www;

            if (www.error == null)
            {
                _ParseMd5File(ref _md5Remote, www.text);
                _md5RemoteFiles = _md5Remote["files"] as JsonObject;
                if (url != _urlList[0]) { 
                    //将成功的url添加到列表前面，认为该url是对用户连接最快的url
                    _urlList.Insert(0, url);
                }
                downloadSucc = true;
                break;
            }
            else {
                DownloadManager.LogWarning("loadlistfile error " + url + MD5_FILE_NAME); 
            }
        }
        if (!downloadSucc) {
            //获取远程md5读取错误时，报错
            _OnNetworkError(www.error);
            yield break;
        }

         DownloadManager.Log("loadend");
         if ((long)(_md5App["force"]) < (long)(_md5Remote["force"]))
         {
            //需要强更
            _OnForceUpdate("" + (_md5Remote["force"]));
        }
        else{
            StartCoroutine(_CheckUpdate());
        }
    }

    
   
    private void _ParseMd5File(ref JsonObject dic, string text)
    {
        dic = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(text);
    }

    private string _GetMd5(JsonObject dic, string filename)
    {
        JsonObject info = dic[filename] as JsonObject;
        if (info == null) {
            return null;
        }
        return info["md5"] as string;
    }

    private IEnumerator _CheckUpdate()
    {
        string random = DateTime.Now.ToString("yyyyMMddhhmmss") + UnityEngine.Random.Range(10000, 99999);

        _taskManager = new DownloadTaskManager();

        long totalsize = 0;
        foreach (KeyValuePair<string, object> kv in _md5RemoteFiles)
        {
            string filename = kv.Key;
            JsonObject fileInfo = kv.Value as JsonObject;
            string remoteValue = fileInfo["md5"] as string;
            string appValue = _GetMd5(_md5AppFiles, filename);
            if (remoteValue == appValue) {
                continue;
            }
            string storageValue = _GetMd5(_md5StorageFiles, filename);
            if (remoteValue == storageValue)
            {
                continue;
            }
            DownloadManager.Log("should Download " + filename);

            string localfile = _desRoot + filename;
             string path = Path.GetDirectoryName(localfile);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

            long size = (long)fileInfo["size"];
            DownloadData data = new DownloadData();
            data.localfile = localfile;
            data.filename = filename;
            data.remoteMd5 = remoteValue;
            data.size = size;
            _taskManager.AddTask(data);

            totalsize += size;
        }
        if (_taskManager.GetEventNum() == 0) { 
            //没有要更新的
            _OnComplete();
            yield break;
        }

        _OnDownloadSize(totalsize); //提示客户端需要更新多少
    }

    private IEnumerator _Download()
    {
        _taskManager.Start(_workThreadCount, _OnFinishOneFile, _OnFailOneFile);
        while (!_taskManager.Fnished())
        {
            _OnProgress(_taskManager.GetRatio());
            yield return new WaitForEndOfFrame();
        }
        _OnProgress(_taskManager.GetRatio());
        _taskManager.Destroy();
        _taskManager = null;

        _md5Remote["hash"] = _md5RemoteHash;
        _md5Remote["done"] = true;
        _SaveMd5StorageFile();
        _SaveMd5RemoteFile();
        _OnComplete();
    }


    private void _OnFinishOneFile(DownloadData data)
    {
        JsonObject info = _md5StorageFiles[data.filename] as JsonObject;
        if (info == null){
            info = new JsonObject();
           _md5StorageFiles[data.filename] = info;
        }
        info["md5"] = data.remoteMd5;
    }

    private void _OnFailOneFile(DownloadData data)
    {
        _OnDownloadFileError(data.filename);
    }

    private void _SaveMd5File(string filename, JsonObject content)
    {
        StreamWriter sw = null;
        string path = FileUtil.Instance.StoragePath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileInfo t = new FileInfo(path + filename);
        try
        {
            sw = t.CreateText();
            string s = content.ToString();
            sw.Write(s);
        }
        catch (Exception)
        {

        }
        finally
        {
            if (sw != null)
            {
                sw.Close();
                sw.Dispose();
            }
        }
    }

    private void _SaveMd5StorageFile()
    {
        _SaveMd5File(MD5_FILE_NAME, _md5Storage);
    }

    private void _SaveMd5RemoteFile()
    {
        _SaveMd5File(MD5_REMOTE_CACHE_NAME, _md5Remote);
    }

    private void _OnNetworkError(string msg)
    {
        DownloadManager.Log(msg);
        if (OnNetworkError != null)
        {
            OnNetworkError.BeginPCall();
            OnNetworkError.PCall();
            OnNetworkError.Push(msg);
            OnNetworkError.EndPCall();;
        }
    }

    private void _OnDownloadFileError(string msg)
    {
        DownloadManager.LogWarning("_OnDownloadFileError " + msg);
        if (OnDownloadFileError != null)
        {
            OnDownloadFileError.BeginPCall();
            OnDownloadFileError.PCall();
            OnDownloadFileError.Push(msg);
            OnDownloadFileError.EndPCall(); ;
        }
    }

    private void _OnForceUpdate(string msg)
    {
        if (OnForceUpdate != null)
        {
            OnForceUpdate.BeginPCall();
            OnForceUpdate.PCall();
            OnForceUpdate.Push(msg);
            OnForceUpdate.EndPCall(); ;
        }
    }

    private void _OnProgress(float ratio)
    {
        DownloadManager.Log("download ratio " + ratio);
        if (OnProgress != null)
        {
            OnProgress.BeginPCall();
            OnProgress.PCall();
            OnProgress.Push(ratio);
            OnProgress.EndPCall(); ;
        }
    }

    private void _OnComplete()
    {
        if (OnComplete != null)
        {
            OnComplete.BeginPCall();
            OnComplete.PCall();
            OnComplete.EndPCall(); ;
        }
    }

    private void _OnDownloadSize(long size)
    {
        DownloadManager.Log("need download " + size);
        if (OnDownloadSize != null)
        {
            OnDownloadSize.BeginPCall();
            OnDownloadSize.PCall();
            OnDownloadSize.Push(size);
            OnDownloadSize.EndPCall(); ;
        }
    }
}

