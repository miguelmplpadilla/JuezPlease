using System;
using Resources.Scripts.Tools;
using UnityEngine;

public class TextIndicatorMouseController : MonoBehaviour
{
    public static TextIndicatorMouseController instance;
    
    [SerializeField] private RectTransform rt;
    [SerializeField] private LocalizableController localizableController;

    public float ySumPosition = 5;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        rt.position = Input.mousePosition + new Vector3(0, ySumPosition, 0);
    }

    public void SetTextIndicator(LocalizableString localizableString)
    {
        localizableController.SetText(localizableString);
        transform.localScale = Vector3.one;
    }

    public void ShowHideTextIndicator(bool show)
    {
        transform.localScale = show ? Vector3.one : Vector3.zero;
    }
}
