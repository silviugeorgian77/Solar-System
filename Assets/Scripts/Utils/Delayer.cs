using System.Collections.Generic;
using UnityEngine;

public class Delayer : MonoBehaviour
{
    public class Delay
    {
        public float DelayDuration { get; set; }
        public float ElapsedTime { get; set; }
        public DelayCallBackFunction DelayCallBack { get; set; }
        public bool ExecuteDelay { get; set; }
        public bool IsCompleted { get; set; }
        public Delay(float delayDuration, DelayCallBackFunction delayCallBack)
        {
            DelayDuration = delayDuration;
            DelayCallBack = delayCallBack;
            ElapsedTime = 0;
            ExecuteDelay = true;
            IsCompleted = false;
        }
    }

    public delegate void DelayCallBackFunction(Delay delayObject);
    private List<Delay> delaysList = new List<Delay>();
    /// <summary>
    /// Used for calling the callbacks in the order they were added.
    /// </summary>
    private List<Delay> delaysToCallCallbacksList = new List<Delay>();
    private Delay currentDelay;
    public Delay AddDelay(float time, DelayCallBackFunction delayCallBack)
    {
        Delay newDelay = new Delay(time, delayCallBack);
        if (time == 0)
        {
            newDelay.DelayCallBack(newDelay);
        }
        else
        {
            delaysList.Add(newDelay);
        }
        return newDelay;
    }

    public void PauseAllDelays()
    {
        foreach (Delay delay in delaysList)
        {
            PauseDelay(delay);
        }
    }

    public void ResumeAllDelays()
    {
        foreach (Delay delay in delaysList)
        {
            ResumeDelay(delay);
        }
    }

    public void PauseDelay(Delay delay)
    {
        delay.ExecuteDelay = false;
    }

    public void ResumeDelay(Delay delay)
    {
        delay.ExecuteDelay = true;
    }

    public void RemoveDelays(List<Delay> delays)
    {
        foreach (var delay in delays)
        {
            RemoveDelay(delay);
        }
    }

    public void RemoveDelay(Delay delay)
    {
        if (delay != null)
        {
            delay.ExecuteDelay = false;
        }
        delaysList.Remove(delay);
    }

    public void RemoveAllDelays()
    {
        delaysList.Clear();
    }

    public int GetDelaysCount()
    {
        return delaysList.Count;
    }

    /// <summary>
    /// Goes through the list in reverse in order to remove passed delays.
    /// Adds the passed delays in a separate list, which is, of course,
    /// in reverse.
    /// Goes through the passed delays list in reverse (to anihilate the initial
    /// reverse) and calls their callback. This is done so that the callbacks
    /// are called in the order their delays were registered.
    /// </summary>
    private void Update()
    {
        delaysToCallCallbacksList.Clear();
        for (int i = delaysList.Count - 1; i >= 0; i--)
        {
            currentDelay = delaysList[i];
            if (currentDelay.ExecuteDelay && !currentDelay.IsCompleted)
            {
                currentDelay.ElapsedTime += Time.deltaTime;
                if (currentDelay.ElapsedTime >= currentDelay.DelayDuration)
                {
                    currentDelay.IsCompleted = true;
                    delaysList.RemoveAt(i);
                    delaysToCallCallbacksList.Add(currentDelay);
                }
            }
        }
        for (int i = delaysToCallCallbacksList.Count - 1; i >= 0; i--)
        {
            currentDelay = delaysToCallCallbacksList[i];
            currentDelay.DelayCallBack(currentDelay);
        }
    }
}
