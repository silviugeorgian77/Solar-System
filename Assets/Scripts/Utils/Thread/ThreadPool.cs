using System.Collections.Generic;

public class ThreadPool : WorkerThread.OnTaskFinishedListener
{
    private LinkedList<WorkerThread.Task> tasks
        = new LinkedList<WorkerThread.Task>();
    private WorkerThread[] threads;

    public ThreadPool(int threadCount)
    {
        threads = new WorkerThread[threadCount];
        WorkerThread thread;
        for (int i = 0; i < threadCount; i++)
        {
            thread = new WorkerThread();
            thread.AddListener(this);           
            threads[i] = thread;
        }
    }

    public void OnTaskFinished(WorkerThread workerThread,
                               WorkerThread.Task finishedTask)
    {
        lock(tasks) {
            if (tasks.Count > 0)
            {
                WorkerThread.Task task = tasks.First.Value;
                tasks.RemoveFirst();
                AddTask(task);
            }
        }
    }

    public void AddTask(WorkerThread.Task task)
    {
        task.Enable();
        lock (tasks) {
            bool taskAdded = false;
            List<WorkerThread.Task> threadTasks;
            foreach (WorkerThread thread in threads)
            {
                threadTasks = thread.GetTasks();
                if (threadTasks.Count == 0)
                {
                    thread.AddTask(task);
                    taskAdded = true;
                    break;
                }
            }
            if (!taskAdded)
            {
                tasks.AddLast(task);
            }
        }
    }

    public void RemoveTask(WorkerThread.Task task)
    {
        task.Disable();
        lock (tasks) {
            foreach (WorkerThread thread in threads)
            {
                thread.RemoveTask(task);
            }
            tasks.Remove(task);
        }
    }

    public void RemoveAllTasks()
    {
        lock(tasks) {
            foreach (WorkerThread thread in threads)
            {
                thread.RemoveAllTasks();
            }
            tasks.Clear();

            foreach (WorkerThread.Task task in tasks)
            {
                task.Disable();
            }
        }
    }

    public List<WorkerThread.Task> GetAllTasks()
    {
        lock(tasks) {
            List<WorkerThread.Task> allTasks = new List<WorkerThread.Task>();
            foreach (WorkerThread thread in threads)
            {
                allTasks.AddRange(thread.GetTasks());
            }
            allTasks.AddRange(tasks);
            return allTasks;
        }
    }
}
