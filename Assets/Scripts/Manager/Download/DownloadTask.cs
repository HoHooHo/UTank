using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

public class DownloadTask
{
    private DownloadTaskManager _manager;
    private DownloadData _currentTask;
    private List<string> _urlList;
    private int _urlIndex;  //在尝试哪个url
    private WebClient _client;
    public DownloadTask(DownloadTaskManager manager)
    {
        _manager = manager;
        _urlList = DownloadManager.instance.urlList;
    }


    public void RunNext()
    {
        DownloadData data = _manager.GetEvent();
        if (data == null)
        {
            //任务全部执行完毕
            return;
        }
        _currentTask = data;
        _urlIndex = 0;
        DownloadFile();
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    void DownloadFile()
    {
        DownloadData data = _currentTask;
        string url = _urlList[_urlIndex] + data.filename;
        string currDownFile = data.localfile;

        DownloadManager.Log("start download " + url);
        //url = "123123";

       _client = new WebClient();

        //sw.Start();
       _client.DownloadFileCompleted += new AsyncCompletedEventHandler(Complete);
       _client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
       _client.DownloadFileAsync(new System.Uri(url), currDownFile);
    }

    private void Complete(object sender, AsyncCompletedEventArgs e)
    {
        if (_client != null) {
            _client.Dispose();
            _client = null;
        }
        DownloadManager.Log("complete");
        if (e.Error != null)
        {
            DownloadManager.LogWarning("download " + _currentTask.filename + " failed " + e.Error);
            _TryAgain();
        }
        else {
            _OnFinish();
        }
    }

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        //DownloadManager.Log(e.ProgressPercentage);

        //if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
        //{
        //    _OnFinish();
        //}
    }

    private void _TryAgain()
    {
        _urlIndex++;
        if (_urlIndex >= _urlList.Count) {
            _manager.FailTask(_currentTask);
            return;
        }
        DownloadFile();
    }

    private void _OnFinish()
    {
        string fileMd5 = FileUtil.Instance.Md5file(_currentTask.localfile);
        if (fileMd5 != _currentTask.remoteMd5) {
            //md5不对，则重新下载
            _TryAgain();
            return;
        }

        _manager.FinishTask(_currentTask);
        RunNext();
    }
}
