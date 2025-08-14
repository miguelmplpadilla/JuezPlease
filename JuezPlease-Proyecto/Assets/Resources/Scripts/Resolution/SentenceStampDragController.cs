using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SentenceStampDragController : StampDragController
{
    [SerializeField] private Vector2 originalRtPosition;

    public RectTransform rt;
    
    public TypeStamp typeStamp;

    public enum TypeStamp
    {
        INOCENT, GUILTY, INSANE
    }

    protected override void Start()
    {
        base.Start();

        EventBus<ReturnToOriginalPositionEvent>.Register(
            new EventBinding<ReturnToOriginalPositionEvent>(StartReturnToOriginalPosition, gameObject));

        originalRtPosition = rt.anchoredPosition;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        EventBus<ReturnToOriginalPositionEvent>.Deregister(
            new EventBinding<ReturnToOriginalPositionEvent>(StartReturnToOriginalPosition, gameObject));
    }

    private void StartReturnToOriginalPosition()
    {
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        if (originalParent.Equals(transform.parent.gameObject)) yield break;
        
        transform.SetParent(originalParent.transform);
        yield return new WaitForEndOfFrame();
        rt.DOAnchorPos(new Vector2(originalRtPosition.x, -Screen.height), 0);
        yield return new WaitForEndOfFrame();

        rt.DOAnchorPosY(originalRtPosition.y, 1);

        yield return new WaitForSeconds(1);
    }
}

public class ReturnToOriginalPositionEvent : IEvent {}
