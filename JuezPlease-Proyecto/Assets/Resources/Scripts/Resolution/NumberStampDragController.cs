using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NumberStampDragController : StampDragController
{
    public Image imageStamp;
    public Sprite spriteStamped;

    [SerializeField] private Vector2 originalRtPosition;

    public RectTransform rt;

    protected override void Start()
    {
        base.Start();

        EventBus<ReturnToOriginalPositionNumberEvent>.Register(
            new EventBinding<ReturnToOriginalPositionNumberEvent>(StartReturnToOriginalPosition, gameObject));

        originalRtPosition = rt.anchoredPosition;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        EventBus<ReturnToOriginalPositionNumberEvent>.Deregister(
            new EventBinding<ReturnToOriginalPositionNumberEvent>(StartReturnToOriginalPosition, gameObject));
    }

    private void StartReturnToOriginalPosition()
    {
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        if (originalParent.Equals(transform.parent.gameObject) ||
            transform.parent.name.Equals("ObjectsContiner")) yield break;
        
        transform.SetParent(originalParent.transform);
        yield return new WaitForEndOfFrame();
        rt.DOAnchorPos(originalRtPosition, 1);

        yield return new WaitForSeconds(1);
        
        Destroy(gameObject);
    }
}

public class ReturnToOriginalPositionNumberEvent : IEvent {}