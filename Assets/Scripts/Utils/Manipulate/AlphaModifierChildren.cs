using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class AlphaModifierChildren : MonoBehaviour
{
    public bool hasAlphaModifiers = true;

    public bool IsInitialized { get; private set; }

    private bool prevHasAlphaModifiers;

    private Dictionary<GameObject, float> initialAlphas
        = new Dictionary<GameObject, float>();

    private void Awake()
    {
        InitIfNeeded();
    }

    private void InitIfNeeded()
    {
        if (!IsInitialized)
        {
            ManageAlphaModifiers();
            IsInitialized = true;
        }
    }

    private void Update()
    {
        ManageAlphaModifiers();
    }

    private void ManageAlphaModifiers()
    {
        if (hasAlphaModifiers != prevHasAlphaModifiers)
        {
            if (hasAlphaModifiers)
            {
                AddAlphaModifiers();
            }
            else
            {
                RemoveAlphaModifiers();
            }
            prevHasAlphaModifiers = hasAlphaModifiers;
        }
    }

    private void AddAlphaModifiers()
    {
        initialAlphas.Clear();

        SpriteRenderer[] spriteRenderers
            = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            AddAlphaModifier(spriteRenderer.gameObject);
            initialAlphas.Add(
                spriteRenderer.gameObject,
                spriteRenderer.color.a
            );
        }

        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh textMesh in textMeshes)
        {
            AddAlphaModifier(textMesh.gameObject);
            initialAlphas.Add(
                textMesh.gameObject,
                textMesh.color.a
            );
        }

        TMP_Text[] tmpTexts = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text tmpText in tmpTexts)
        {
            AddAlphaModifier(tmpText.gameObject);
            initialAlphas.Add(
                tmpText.gameObject,
                tmpText.color.a
            );
        }
    }

    private void AddAlphaModifier(GameObject gameObject)
    {
        if (gameObject.GetComponent<AlphaModifier>() == null)
        {
            gameObject.AddComponent(typeof(AlphaModifier));
        }
    }

    private void RemoveAlphaModifiers()
    {
        initialAlphas.Clear();

        AlphaModifier[] alphaModifiers
            = GetComponentsInChildren<AlphaModifier>();
        foreach (AlphaModifier alphaModifier in alphaModifiers)
        {
            if (Application.isPlaying)
            {
                Destroy(alphaModifier);
            }
            else
            {
                DestroyImmediate(alphaModifier);
            }
        }
    }

    public void AlphaTo(float alpha,
       float time,
       EaseEquations.EaseFunctionDelegate easeFunction = null)
    {
        InitIfNeeded();

        AlphaModifier[] alphaModifiers
           = GetComponentsInChildren<AlphaModifier>();
        foreach (AlphaModifier alphaModifier in alphaModifiers)
        {
            if (alphaModifier.ExecuteInAlphaModifierChildren)
            {
                alphaModifier.AlphaTo(alpha, time, easeFunction, null);
            }       
        }
    }

    public void AlphaTo(float alpha)
    {
        InitIfNeeded();

        Color color;
        AlphaModifier alphaModifier;

        SpriteRenderer[] spriteRenderers
            = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            alphaModifier = spriteRenderer.GetComponent<AlphaModifier>();
            if (alphaModifier.ExecuteInAlphaModifierChildren)
            {
                color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }

        TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh textMesh in textMeshes)
        {
            alphaModifier = textMesh.GetComponent<AlphaModifier>();
            if (alphaModifier.ExecuteInAlphaModifierChildren)
            {
                color = textMesh.color;
                color.a = alpha;
                textMesh.color = color;
            }
        }

        TMP_Text[] tmpTexts = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text tmpText in tmpTexts)
        {
            alphaModifier = tmpText.GetComponent<AlphaModifier>();
            if (alphaModifier.ExecuteInAlphaModifierChildren)
            {
                color = tmpText.color;
                color.a = alpha;
                tmpText.color = color;
            }
        }
    }

    public void AlphaToInitial(
       float time,
       EaseEquations.EaseFunctionDelegate easeFunction = null,
       AlphaModifier.AlphaEndedCallBackFunction EndCallBack = null)
    {
        InitIfNeeded();

        AlphaModifier[] alphaModifiers
                   = GetComponentsInChildren<AlphaModifier>();
        foreach (AlphaModifier alphaModifier in alphaModifiers)
        {
            if (alphaModifier.ExecuteInAlphaModifierChildren)
            {
                alphaModifier.AlphaTo(
                    initialAlphas[alphaModifier.gameObject],
                    time,
                    easeFunction,
                    EndCallBack
                );
            }
        }
    }
}
