using System;
using System.Collections;
using DG.Tweening;
using Resources.Scripts.Hammer;
using UnityEngine;
using UnityEngine.UI;

public class PressFalseController : MonoBehaviour
{
    public Button pressFalseButton;

    public RectTransform pressRt;
    public RectTransform falseButtonRt;

    public GameObject positionRaycastFalse;

    private bool showed = false;
    private bool isAnimating = false;

    private bool canForward = true;

    public GameObject prefabStampFalse;

    public BoxCollider2D collider;

    private void Start()
    {
        pressFalseButton.onClick.AddListener(() => { StartCoroutine(PressFalse()); });
    }

    public void ForwardOpenClose()
    {
        if (!canForward || isAnimating) return;
        
        pressRt.DOKill();
        pressRt.DOAnchorPosY(!showed ? 20 : 148, 0.3f);
        canForward = false;
    }

    public void StartBackwardOpenClose()
    {
        StartCoroutine(BackwardOpenClose());
    }
    
    public IEnumerator BackwardOpenClose()
    {
        canForward = true;
        
        if (isAnimating) yield break;
        
        pressRt.DOKill();
        pressRt.DOAnchorPosY(showed ? 168 : 0, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    public void StartShowHidePress()
    {
        StartCoroutine(ShowHidePress());
    }

    private IEnumerator ShowHidePress()
    {
        if (isAnimating) yield break;
        
        isAnimating = true;
        pressRt.DOAnchorPosY(!showed ? 168 : 0, 0.5f);
        showed = !showed;
        yield return new WaitForSeconds(0.5f);
        isAnimating = false;
    }

    private IEnumerator PressFalse()
    {
        if (isAnimating) yield break;
        
        isAnimating = true;
        falseButtonRt.DOAnchorPosY(-20, 0.05f);
        yield return new WaitForSeconds(0.05f);
        StampFalse();
        yield return new WaitForSeconds(0.1f);
        falseButtonRt.DOAnchorPosY(0, 0.05f);
        yield return new WaitForSeconds(0.05f);
        isAnimating = false;
    }

    private void StampFalse()
    {
        EventBus<SlamHammerAnimationEvent>.Raise(new SlamHammerAnimationEvent());
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(positionRaycastFalse.transform.position, collider.size, 0f, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Document"))
            {
                StartCoroutine(CreateStamped(hit.collider.gameObject));
                return;
            }
        }
    }

    private IEnumerator CreateStamped(GameObject parent)
    {
        DragObjectController dragObject = parent.GetComponent<DragObjectController>();
        if (!dragObject.document.canBeFalse) yield break;
        
        GameObject stamp = Instantiate(prefabStampFalse);
        stamp.transform.position = positionRaycastFalse.transform.position;
        yield return null;
        stamp.transform.SetParent(parent.transform.Find("AllImages").Find("ImagesBig").Find("ContinerStamps"));
        
        if (dragObject.document.isMarkedHasFalse) yield break;
        
        dragObject.document.isMarkedHasFalse = true;
            
        Statment statmentFalse = ScriptableObject.CreateInstance<Statment>();
        statmentFalse.textStatment = dragObject.document.dialogueFalse.textStatment;
        statmentFalse.dialogue = dragObject.document.isFalse
            ? dragObject.document.dialogueFalse.dialogue
            : StatmentsController.instance.GetNotFalseDialogue(dragObject.document.principalSpeaker);
            
        StatmentsController.instance.CreateNewStatment(statmentFalse);
    }
}
