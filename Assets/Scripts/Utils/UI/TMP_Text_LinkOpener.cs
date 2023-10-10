using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The link must have "http://" or "https://" in front, otherwise
/// it won't be opened.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TMP_Text_LinkOpener : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The camera of the canvas. For space overlay canvas, leave null.
    /// </summary>
    [SerializeField]
    private Camera referenceCamera;

    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text pTextMeshPro = GetComponent<TMP_Text>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            pTextMeshPro,
            eventData.position,
            referenceCamera
        );
        if (linkIndex != -1)
        {
            // Link was clicked
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            var linkId = linkInfo.GetLinkID();
            Application.OpenURL(linkId);
        }
    }

}