using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

public class DownloadData
{
    public string filename;
    public string url;
    public string localfile;
    public string remoteMd5;
    public long size;
}

public delegate void DownloadDataCallback(DownloadData data);

public class DownloadTaskManager
{
    private List<DownloadTask> _taskList;
    private int _workNum;
    private DownloadDataCallback _onFinishOneFile;
    private DownloadDataCallback _onFailedOneFile;

    public Queue<DownloadData> events = new Queue<DownloadData>();

    static readonly object m_lockObj = new object();

    public int totalNum;
    public int finishNum;

    public void Start(int workNum, DownloadDataCallback onFinishOneFile, DownloadDataCallback onFailedOneFile)
    {
        _onFinishOneFile = onFinishOneFile;
        _onFailedOneFile = onFailedOneFile;
        _workNum = workNum;
        this.totalNum = GetEventNum();
        this.finishNum = 0;
        _taskList = new List<DownloadTask>();
        for (int i = 0; i < workNum; i++)
        {
            DownloadTask task = new DownloadTask(this);
            _taskList.Add(task);
            task.RunNext();
        }
    }

    public void Destroy()
    {
        _taskList = null;
    }

    /// <summary>
    /// 添加到事件队列
    /// </summary>
    public void AddTask(DownloadData data)
    {
        events.Enqueue(data);
    }

    public int GetEventNum()
    {
        return events.Count;
    }

    public DownloadData GetEvent()
    {
        lock (m_lockObj) {
            if (events.Count == 0)
            {
                return null;
            }
            return events.Dequeue();
        }
    }

    public void FinishTask(DownloadData task)
    {
        lock (m_lockObj)
        {
            this.finishNum++;
            if (_onFinishOneFile != null) {
                _onFinishOneFile(task);
            }
        }
    }

    public void FailTask(DownloadData task)
    {
        lock (m_lockObj)
        {
            this.finishNum++;   //出错的时候也把数量+1吧，不然停止不了了
            if (_onFailedOneFile != null)
            {
                _onFailedOneFile(task);
            }
        }
    }

    public bool Fnished()
    {
        lock (m_lockObj)
        {
            return this.finishNum >= this.totalNum;
        }
    }

    public float GetRatio()
    {
        lock (m_lockObj)
        {
            if (this.totalNum == 0) {
                return 1.0f;
            }
            return (float)(this.finishNum) / (float)(this.totalNum);
        }
    }
}
