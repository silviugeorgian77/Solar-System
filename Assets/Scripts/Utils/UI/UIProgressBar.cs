using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image progressImage;

    private float progress;
    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value;
            progressImage.fillAmount
                = (progress / 1f)
                    .ClampValue(0f, 1f);
        }
    }
}
