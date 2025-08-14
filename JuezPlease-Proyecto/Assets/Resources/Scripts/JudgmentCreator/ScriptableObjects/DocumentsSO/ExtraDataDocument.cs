using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class AllExtraDataDocument
{
    public List<ExtraDataDocument> allExtraData = new List<ExtraDataDocument>();
}

[Serializable]
public class ExtraDataDocument
{
    public string guidDocument;
    public List<ExtraData> extraData = new List<ExtraData>();
}

[Serializable]
public class ExtraData
{
    [HideInInspector] public RectTransformData rtData = null;
    [HideInInspector] public int siblingIndex;
    
    public Sprite sprite;
    public TextExtraData text;
}

[Serializable]
public class RectTransformData
{
    public Vector2 anchoredPosition;
    public Vector2 sizeDelta;
    public Vector2 pivot;
    public Vector2 anchorMin;
    public Vector2 anchorMax;

    public Vector3 rotation;
    public Vector3 scale;

    public RectTransformData(RectTransform rectTransform)
    {
        anchoredPosition = rectTransform.anchoredPosition;
        sizeDelta = rectTransform.sizeDelta;
        pivot = rectTransform.pivot;
        anchorMin = rectTransform.anchorMin;
        anchorMax = rectTransform.anchorMax;
        rotation = rectTransform.localEulerAngles;
        scale = rectTransform.localScale;
    }
}

[Serializable]
public class TextExtraData
{
    public LocalizableString dataText;
    public TMP_FontAsset fontText;
}
