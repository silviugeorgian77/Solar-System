using UnityEngine;

public class FrameWaiter : MonoBehaviour
{
    private bool executeWaitForFrames = false;
    public delegate void WaitForNextFrameCallBackFunction();
    private WaitForNextFrameCallBackFunction WaitForNextFrameCallBack;
    private WaitForNextFrameCallBackFunction CurrentWaitForNextFrameCallBack;
    private int passedFrames;
    private int framesToWait;
    public void WaitForFrames(int framesToWait,
        WaitForNextFrameCallBackFunction waitForNextFrameCallBack)
    {
        if (framesToWait == 0)
        {
            if (waitForNextFrameCallBack != null)
            {
                waitForNextFrameCallBack();
            }
            return;
        }
        this.framesToWait = framesToWait;
        passedFrames = 0;
        executeWaitForFrames = true;
        WaitForNextFrameCallBack = waitForNextFrameCallBack;
    }

    private void Update()
    {
        if (executeWaitForFrames)
        {
            passedFrames += 1;
            if (passedFrames > framesToWait)
            {
                executeWaitForFrames = false;
                if (WaitForNextFrameCallBack != null)
                {
                    CurrentWaitForNextFrameCallBack = WaitForNextFrameCallBack;
                    WaitForNextFrameCallBack = null;
                    CurrentWaitForNextFrameCallBack();
                }
            }
        }
    }
}
