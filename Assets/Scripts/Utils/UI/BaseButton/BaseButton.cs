using UnityEngine;

public class BaseButton : MonoBehaviour
{
    public Touchable touchable;
    public delegate void ClickCallBackFunction(BaseButton baseButton);
    public ClickCallBackFunction clickCallBack;
    public bool ButtonEnabled { get; set; } = true;
    public AudioClip pressedAudioClip;
    public AudioClip unpressedAudioClip;

    protected virtual void Awake()
    {
        touchable.OnClickStartedInsideCallBack = ExecuteOnClickStartedInside;
        touchable.OnClickEndedInsideCallBack = ExecuteOnClickEndedInside;
        touchable.OnClickEndedOutsideCallBack = ExecuteOnClickEndedOutside;
        touchable.OnClickHoldingOutsideCallBack = ExecuteOnClickHoldingOutside;
    }

    private void ExecuteOnClickStartedInside(Touchable touchable)
    {
        if (!ButtonEnabled)
        {
            return;
        }
        OnClickStartedInside(touchable);
        if (pressedAudioClip != null)
        {
            Singleton
                .GetFirst<AudioManager>()
                .PlayFX(pressedAudioClip);
        }
    }

    protected virtual void OnClickStartedInside(Touchable touchable) { }

    private void ExecuteOnClickEndedInside(Touchable touchable)
    {
        if (!ButtonEnabled)
        {
            return;
        }
        OnClickEndedInside(touchable);
        if (unpressedAudioClip != null)
        {
            Singleton
                .GetFirst<AudioManager>()
                .PlayFX(unpressedAudioClip);
        }
    }

    protected virtual void OnClickEndedInside(Touchable touchable)
    {
        if (!ButtonEnabled)
        {
            return;
        }
        clickCallBack?.Invoke(this);
    }

    private void ExecuteOnClickEndedOutside(Touchable touchable)
    {
        if (!ButtonEnabled)
        {
            return;
        }
        OnClickEndedOutside(touchable);
    }

    protected virtual void OnClickEndedOutside(Touchable touchable) { }

    private void ExecuteOnClickHoldingOutside(Touchable touchable)
    {
        if (!ButtonEnabled)
        {
            return;
        }
        OnClickHoldingOutside(touchable);
    }

    protected virtual void OnClickHoldingOutside(Touchable touchable) { }
}
