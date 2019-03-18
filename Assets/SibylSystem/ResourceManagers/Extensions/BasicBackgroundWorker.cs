using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>Basic Background Worker Class that queues Actions and execute them in another thread.</summary>
public class BasicBackgroundWorker
{
    private readonly Thread _backgroundWorkThread;
    private readonly Queue<Action> _queue = new Queue<Action>();
    private readonly ManualResetEvent _workAvailable = new ManualResetEvent(false);

    public BasicBackgroundWorker()
    {
        _backgroundWorkThread = new Thread(BackgroundThread)
        {
            IsBackground = true,
            Priority = System.Threading.ThreadPriority.BelowNormal,
            Name = "BasicBackgroundWorker Thread"
        };
        _backgroundWorkThread.Start();
    }

    /// <summary>Enqueues the work.</summary>
    /// <param name="work">The work.</param>
    public void EnqueueWork(Action work)
    {
        lock (_queue)
        {
            _queue.Enqueue(work);
            _workAvailable.Set();
        }
    }

    private void BackgroundThread()
    {
        while (true)
        {
            _workAvailable.WaitOne();
            Action workItem;
            lock (_queue)
            {
                workItem = _queue.Dequeue();
                if (_queue.Count == 0)
                {
                    _workAvailable.Reset();
                }
            }
            try
            {
                workItem();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}