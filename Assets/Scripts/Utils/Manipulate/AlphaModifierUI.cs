using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlphaModifierUI : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private RawImage rawImage;

    [SerializeField]
    private TMP_Text tmpText;

    [SerializeField]
    private bool autoAssignReferences = false;

    public bool executeInAlphaModifierChildren = true;

    private bool executeAlpha = false;
    public float durationAlpha;
    private float startAlpha;
    private float finalAlpha;
    private float elapsedTimeAlpha;
    private float changeInValueAlpha;
    private EaseEquations.EaseFunctionDelegate easeFunctionAlpha;
    public delegate void AlphaEndedCallBackFunction();
    private AlphaEndedCallBackFunction AlphaEndedCallBack;
    private float currentAlpha;
    private Color color = new Color();

    public virtual void Awake()
    {
        if (autoAssignReferences)
        {
            image = GetComponent<Image>();
            tmpText = GetComponent<TMP_Text>();
            rawImage = GetComponent<RawImage>();
        }
    }

    public void AlphaTo(float alpha,
        float time,
        EaseEquations.EaseFunctionDelegate easeFunction = null,
        AlphaEndedCallBackFunction EndCallBack = null)
    {
        AlphaStop();
        AlphaEndedCallBack = EndCallBack;
        if (time == 0)
        {
            if (image != null)
            {
                color.r = image.color.r;
                color.g = image.color.g;
                color.b = image.color.b;
                color.a = alpha;
                image.color = color;
            }
            else if (tmpText != null)
            {
                color.r = tmpText.color.r;
                color.g = tmpText.color.g;
                color.b = tmpText.color.b;
                color.a = alpha;
                tmpText.color = color;
            }
            else if (rawImage != null)
            {
                color.r = rawImage.color.r;
                color.g = rawImage.color.g;
                color.b = rawImage.color.b;
                color.a = alpha;
                rawImage.color = color;
            }
            OnFinished();
            return;
        }
        if (easeFunction == null)
        {
            easeFunction = EaseEquations.noEaseFunction;
        }
        finalAlpha = alpha;
        if (image != null)
        {
            startAlpha = image.color.a;
        }
        else if (tmpText != null)
        {
            startAlpha = tmpText.color.a;
        }
        else if (rawImage != null)
        {
            startAlpha = rawImage.color.a;
        }
        changeInValueAlpha = finalAlpha - startAlpha;
        executeAlpha = true;
        elapsedTimeAlpha = 0;
        durationAlpha = time;
        easeFunctionAlpha = easeFunction;
    }
    public void AlphaStop()
    {
        executeAlpha = false;
    }

    private void Update()
    {
        if (executeAlpha)
        {
            elapsedTimeAlpha += Time.deltaTime;
            currentAlpha = easeFunctionAlpha(
                changeInValueAlpha,
                elapsedTimeAlpha,
                durationAlpha,
                startAlpha
            );
            if (image != null)
            {
                color.r = image.color.r;
                color.g = image.color.g;
                color.b = image.color.b;
                color.a = currentAlpha;
                image.color = color;
            }
            else if (tmpText != null)
            {
                color.r = tmpText.color.r;
                color.g = tmpText.color.g;
                color.b = tmpText.color.b;
                color.a = currentAlpha;
                tmpText.color = color;
            }
            else if (rawImage != null)
            {
                color.r = rawImage.color.r;
                color.g = rawImage.color.g;
                color.b = rawImage.color.b;
                color.a = currentAlpha;
                rawImage.color = color;
            }
            if (elapsedTimeAlpha >= durationAlpha)
            {
                executeAlpha = false;
                if (image != null)
                {
                    color.r = image.color.r;
                    color.g = image.color.g;
                    color.b = image.color.b;
                    color.a = finalAlpha;
                    image.color = color;
                }
                else if (tmpText != null)
                {
                    color.r = tmpText.color.r;
                    color.g = tmpText.color.g;
                    color.b = tmpText.color.b;
                    color.a = finalAlpha;
                    tmpText.color = color;
                }
                else if (rawImage != null)
                {
                    color.r = rawImage.color.r;
                    color.g = rawImage.color.g;
                    color.b = rawImage.color.b;
                    color.a = finalAlpha;
                    rawImage.color = color;
                }
                OnFinished();
            }
        }
    }

    private void OnFinished()
    {
        AlphaEndedCallBack?.Invoke();
    }
}
