using System.Collections;
using UnityEngine;

public class PreloadController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.3f);
        
        EventBus<TransitionSceneEvent>.Raise(new TransitionSceneEvent
        {
            sceneNameToTransition = "JudgedScene",
            playTransition = false
        });
    }
}
