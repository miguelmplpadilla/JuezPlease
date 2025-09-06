using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    public GameObject eyeLids;

    public RectTransform eyesRt;

    public Animator animator;

    public DialogController.TypeSpeaker speaker;
    
    public GameObject iconConversation;
    public bool canInteract = false;
    
    private void Start()
    {
        EventBus<InteractNPCEvent>.Register(new EventBinding<InteractNPCEvent>(Interact, gameObject));
        EventBus<AboveInteractNPCEvent>.Register(new EventBinding<AboveInteractNPCEvent>(AboveInteract, gameObject));
        
        EventBus<LookToEvent>.Register(new EventBinding<LookToEvent>(LookTo, gameObject));
        EventBus<PlayAnimationNPCEvent>.Register(new EventBinding<PlayAnimationNPCEvent>(PlayAnimationNPC, gameObject));
        
        StartCoroutine(Blink());
    }
    
    private void OnDestroy()
    {
        EventBus<InteractNPCEvent>.Deregister(new EventBinding<InteractNPCEvent>(Interact, gameObject));
        EventBus<AboveInteractNPCEvent>.Deregister(new EventBinding<AboveInteractNPCEvent>(AboveInteract, gameObject));
        
        EventBus<LookToEvent>.Deregister(new EventBinding<LookToEvent>(LookTo, gameObject));
        EventBus<PlayAnimationNPCEvent>.Deregister(new EventBinding<PlayAnimationNPCEvent>(PlayAnimationNPC, gameObject));
    }
    
    private void AboveInteract(AboveInteractNPCEvent i)
    {
        if (!i.obj.Equals(gameObject)) return;

        canInteract = i.canInteract;
    }

    private void Interact(InteractNPCEvent i)
    {
        if (!i.obj.Equals(gameObject)) return;
        
        EventBus<SendDialogEvent>.Raise(new SendDialogEvent
        {
            dialogueStartNode = i.dialogueNode,
            document = i.document
        });
    }

    private void Update()
    {
        iconConversation.transform.localScale = canInteract ? Vector3.one : Vector3.zero;
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return null;
            yield return new WaitForSeconds(Random.Range(2, 10 + 1));
            eyeLids.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.05f);
            eyeLids.transform.localScale = Vector3.zero;
        }
    }

    private void PlayAnimationNPC(PlayAnimationNPCEvent p)
    {
        if (!p.obj.Equals(gameObject.transform.parent.gameObject)) return;

        foreach (var emotion in p.emotions)
        {
            string animationName = emotion.emotion.ToString().ToLower() + emotion.arm.ToString().ToLower();
            Debug.Log("Play Animation: "+animationName);
            //animator.SetTrigger(animationName);
            if (animator.HasState((int)emotion.arm, Animator.StringToHash(animationName)))
            {
                animator.SetLayerWeight((int)emotion.arm, 1);
                animator.Play(animationName, (int)emotion.arm, 0);
            }
            else
            {
                Debug.LogWarning("Animation not found: " + animationName);
            }
        }
    }

    private void LookTo(LookToEvent l)
    {
        return;
        if (l.lookToJudge)
        {
            eyesRt.DOAnchorPosX(10, 0);
            return;
        }

        eyesRt.DOAnchorPosX(l.objToLook.transform.position.x > transform.position.x ? 12 : 7.4f, 0);
    }
}
