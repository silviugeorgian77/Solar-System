using UnityEngine;
using UnityEngine.UI;

public class ButtonToggleGroupUI : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;

    [SerializeField]
    private Button selectedButton;

    public bool IsInitialized { get; private set; }

    public Button[] Buttons
    {
        get
        {
            return buttons;
        }
    }

    public Button SelectedButton
    {
        get
        {
            return selectedButton;
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                foreach (Button currentButton in buttons)
                {
                    if (currentButton != button)
                    {
                        currentButton.interactable = true;
                    }
                }
                button.interactable = false;
            });
        }

        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }

        IsInitialized = true;
    }
}
