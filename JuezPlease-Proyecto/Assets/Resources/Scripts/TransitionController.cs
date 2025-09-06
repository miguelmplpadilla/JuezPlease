using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    public RectTransform topRt;
    public RectTransform bottomRt;

    public CanvasGroup canvasGroup;
    
    void Awake()
    {
        EventBus<TransitionSceneEvent>.Register(new EventBinding<TransitionSceneEvent>(PlayTransitionScene, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<TransitionSceneEvent>.Deregister(new EventBinding<TransitionSceneEvent>(PlayTransitionScene, gameObject));
    }

    private void PlayTransitionScene(TransitionSceneEvent t)
    {
        StartCoroutine(TransitionScene(t));
    }
    
    private IEnumerator TransitionScene(TransitionSceneEvent t)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        topRt.DOAnchorPosX(0, t.playTransition ? 1 : 0);
        bottomRt.DOAnchorPosX(0, t.playTransition ? 1 : 0);

        yield return new WaitForSeconds(t.playTransition ? 1 : 0);
        
        AsyncOperation asyncOperation =
            SceneManager.LoadSceneAsync(t.sceneNameToTransition, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            Debug.Log($"Progreso de carga: {asyncOperation.progress * 100}%");
            yield return null;
        }

        if (!t.currentSceneName.Equals("")) 
            yield return SceneManager.UnloadSceneAsync(t.currentSceneName).GetAwaiter();

        yield return new WaitForSeconds(0.5f);
        
        topRt.DOAnchorPosX(Screen.width, 1);
        bottomRt.DOAnchorPosX(-Screen.width, 1);

        yield return new WaitForSeconds(1);
        
        topRt.DOAnchorPosX(-Screen.width, 0);
        bottomRt.DOAnchorPosX(Screen.width, 0);
        
        t.callback?.Invoke();
        
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}

public class TransitionSceneEvent : IEvent
{
    public bool playTransition = true;
    public string currentSceneName = "";
    public string sceneNameToTransition = "";

    public Action callback = null;
}
