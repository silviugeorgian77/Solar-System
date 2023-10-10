using System.Collections.Generic;
using System.Threading;

public class WorkerThread
{
    private SynchronizationContext context;
    private bool enabled = true;
    private Task currentTask;
    private List<Task> tasks = new List<Task>();
    private List<OnTaskFinishedListener> listeners
        = new List<OnTaskFinishedListener>();

    public WorkerThread()
    {
        context = SynchronizationContext.Current;
        if (context == null)
        {
            context = new SynchronizationContext();
        }
        Thread t = new Thread(Run);
#if !UNITY_EDITOR
        t.IsBackground = true;
#endif
        t.Start();
    }

    public void AddTask(Task task)
    {
        task.Enable();
        lock (tasks) {
            tasks.Add(task);
            Monitor.Pulse(tasks);
        }
    }

    public void RemoveTask(Task task)
    {
        task.Disable();
        lock (tasks) {
            tasks.Remove(task);
        }
    }

    public void RemoveAllTasks()
    {
        lock(tasks) {
            foreach (Task task in tasks)
            {
                task.Disable();
            }
            tasks.Clear();
        }
    }

    public void AddListener(OnTaskFinishedListener listener)
    {
        lock(listeners) {
            listeners.Add(listener);
        }
    }

    public void RemoveListener(OnTaskFinishedListener listener)
    {
        lock(listeners) {
            listeners.Remove(listener);
        }
    }

    public void RemoveAllListeners()
    {
        lock(listeners) {
            listeners.Clear();
        }
    }

    public SynchronizationContext GetContext()
    {
        return context;
    }

    public List<Task> GetTasks()
    {
        lock (tasks) {
            return tasks;
        }
    }

    public bool IsEnabled()
    {
        return enabled;
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    
    public void Run()
    {
        while (true)
        {
            lock (tasks) {
                if (tasks.Count > 0)
                {
                    currentTask = tasks[0];
                }
                else
                {
                    Monitor.Wait(tasks);
                }
            }
            if (currentTask != null)
            {
                currentTask.Execute();
            }
            if (currentTask != null)
            {
                lock (tasks)
                {
                    tasks.Remove(currentTask);
                }
                context.Send((o) => {
                    currentTask.OnFinished();
                    lock (listeners)
                    {
                        foreach (OnTaskFinishedListener listener
                            in listeners)
                        {
                            listener.OnTaskFinished(this, currentTask);
                        }
                    }
                },
                null);
                currentTask = null;
            }
        }
    }

    public interface Task
    {
        void Execute();
        void OnFinished();
        void Enable();
        void Disable();
    }

    public interface OnTaskFinishedListener
    {
        void OnTaskFinished(WorkerThread workerThread, Task finishedTask);
    }
}
