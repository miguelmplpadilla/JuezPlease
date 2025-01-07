using UnityEngine;

public class AutoScalerByAspectRatio : MonoBehaviour
{
    private void Awake()
    {
        float normalAspectRatio = (float)1920 / 1080;
        float currentAspectRatio = (float)Screen.width / Screen.height;

        transform.localScale = Vector3.one * (normalAspectRatio / currentAspectRatio);
    }
}
