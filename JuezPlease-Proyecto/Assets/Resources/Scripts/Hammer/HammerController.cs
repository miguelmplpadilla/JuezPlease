using System.Collections;
using DG.Tweening;
using Resources.Scripts.Hammer;
using UnityEngine;
using UnityEngine.UI;

public class HammerController : MonoBehaviour
{
    private Animator hammer;

    public GameObject dots;
    
    public int indexDots = 0;
    
    private Coroutine coroutine;
    
    public void SlamHammer()
    {
        if (indexDots >= dots.transform.childCount) return;
        
        EventBus<SlamHammerAnimationEvent>.Raise(new SlamHammerAnimationEvent());
        
        if (coroutine != null)
            StopCoroutine(coroutine);

        dots.transform.GetChild(indexDots).GetComponent<Image>().DOFade(1, 0.3f);
        
        //hammer.SetTrigger("Slam");
        
        indexDots++;
        
        if (indexDots >= dots.transform.childCount)
        {
            EventBus<TransitionSceneEvent>.Raise(new TransitionSceneEvent
            {
                currentSceneName = "JudgedScene",
                sceneNameToTransition = "ResolutionScene"
            });
            return;
        }

        if (indexDots < dots.transform.childCount)
            coroutine = StartCoroutine(RestartDots());
    }

    private IEnumerator RestartDots()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < dots.transform.childCount; i++)
        {
            Image image = dots.transform.GetChild(i).GetComponent<Image>();
            image.DOComplete();
            image.DOKill();
            image.DOFade(0, 0);
        }

        indexDots = 0;
    }
}
