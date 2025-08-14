using UnityEngine;

public class PreloadController : MonoBehaviour
{
    void Start()
    {
        EventBus<TransitionSceneEvent>.Raise(new TransitionSceneEvent
        {
            sceneNameToTransition = "JudgedScene",
            playTransition = false
        });
    }
}
