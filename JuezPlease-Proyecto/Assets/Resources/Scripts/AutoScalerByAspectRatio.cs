using System;
using UnityEngine;

public class AutoScalerByAspectRatio : MonoBehaviour
{
    private Vector3 originalScale = Vector3.one;

    private void Start()
    {
        originalScale = transform.localScale;
        
        float normalAspectRatio = (float)1920 / 1080;
        float currentAspectRatio = (float)Screen.width / Screen.height;

        transform.localScale = originalScale * (normalAspectRatio / currentAspectRatio);
    }
}
