using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts.Hammer;
using Resources.Scripts.Holder;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObjectController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<Conversation> posibleConversations = new List<Conversation>();
    
    public float multiplyScaleForBig = 5;

    protected float originalScale = 1;
    protected float scaleBig = 1;

    public RectTransform[] imagesRectTransform;
    public RectTransform[] shadowsRectTransform;

    public GameObject imagesLittle;
    public GameObject imagesBig;

    public GameObject allImages;
    protected GameObject continer;
    protected GameObject currentParent;
    protected GameObject originalParent;
    protected GameObject canvas;

    protected bool isAnimating = false;
    protected bool isDragging = false;

    protected bool canJump = true;

    private Sequence sequenceSlamHammer;
    
    public string objTag = "BigObjects";

    private void Awake()
    {
        originalParent = transform.parent.gameObject;
    }
    
    protected virtual void Start()
    {
        EventBus<SlamHammerAnimationEvent>.Register(
            new EventBinding<SlamHammerAnimationEvent>(HammerSlamJump, gameObject));
        
        canvas = GameObject.Find("CanvasTable");
        originalScale = transform.localScale.x;
        scaleBig = originalScale * multiplyScaleForBig;
    }

    private void OnDestroy()
    {
        EventBus<SlamHammerAnimationEvent>.Deregister(
            new EventBinding<SlamHammerAnimationEvent>(HammerSlamJump, gameObject));
    }

    protected virtual void Update()
    {
        if (!isAnimating && isDragging)
            SetData();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        isDragging = true;
        
        foreach (var shadow in shadowsRectTransform)
        {
            shadow.DOComplete();
            shadow.DOKill();
            shadow.DOAnchorPos(new Vector2(5, -5), 0.2f);
        }
        
        allImages.transform.DOScale(1.05f, 0.1f);
        
        transform.SetParent(canvas.transform);

        if (currentParent == null) return;
        
        EventBus<SetObjectToListEvent>.Raise(new SetObjectToListEvent
        {
            obj = gameObject,
            objHolder = currentParent,
            add = false
        });
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        isDragging = true;
        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        foreach (var shadow in shadowsRectTransform)
        {
            shadow.DOComplete();
            shadow.DOKill();
            shadow.DOAnchorPos(Vector2.zero, 0.2f);
        }
        allImages.transform.DOScale(1, 0.1f);
        
        transform.SetParent(continer.transform);
        
        EventBus<SetObjectToListEvent>.Raise(new SetObjectToListEvent
        {
            obj = gameObject,
            objHolder = currentParent,
            add = true
        });
        
        isDragging = false;
        
        GlobalOnEndDrag();
    }

    protected void CheckNPCDraged()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            { position = transform.position };
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var obj in results)
        {
            if (obj.gameObject.CompareTag("NPC"))
            {
                EventBus<InteractNPCEvent>.Raise(new InteractNPCEvent
                {
                    obj = obj.gameObject,
                    objConversation = gameObject
                });
                
                return;
            }
        }
    }

    protected virtual void GlobalOnEndDrag()
    {
        if (!currentParent.name.Equals("PublicPanel")) return;
        
        isAnimating = true;
        transform.DOMoveY(GameObject.Find("YEndTable").transform.position.y, 0.2f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                isAnimating = false;
            }).OnUpdate(() =>
            {
                SetData();
            });
        
        CheckNPCDraged();
    }

    protected void SetData()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        { position = transform.position };
        EventSystem.current.RaycastAll(pointerEventData, results);
        
        transform.SetAsLastSibling();

        foreach (var result in results)
        {
            HolderController holderController = result.gameObject.GetComponent<HolderController>();
            if (holderController != null)
            {
                SetDataObject(result.gameObject, holderController, result.gameObject.CompareTag(objTag));
                return;
            }
        }
    }

    protected void SetDataObject(GameObject result, HolderController holderController, bool isBig)
    {
        if (result.gameObject.Equals(currentParent)) return;
        
        GameObject currentImages = isBig ? imagesBig : imagesLittle;
        
        imagesBig.transform.localScale = Vector3.zero;
        imagesLittle.transform.localScale = Vector3.zero;
        currentImages.transform.localScale = Vector3.one;
        
        currentParent = result;
        transform.localScale = (isBig ? scaleBig : originalScale) * Vector3.one;
                
        continer = holderController.objContiner;
    }

    private void HammerSlamJump()
    {
        if (!canJump) return;
        
        if (sequenceSlamHammer != null)
        {
            sequenceSlamHammer.Complete();
            sequenceSlamHammer.Kill();
        }
        
        Sequence sequenceMove = DOTween.Sequence();
        Sequence sequenceScale = DOTween.Sequence();
        
        sequenceSlamHammer = DOTween.Sequence();
        sequenceSlamHammer.SetEase(Ease.Linear);
        
        foreach (var image in imagesRectTransform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOAnchorPosY(5, 0.05f));
            sequence.Append(image.DOAnchorPosY(0, 0.05f));
            
            sequenceMove.Join(sequence);
        }

        float originalXScale = transform.localScale.x;
        
        var scale1 = transform.DOScale(originalXScale + 0.05f, 0.05f);
        var scale2 = transform.DOScale(originalXScale, 0.05f);
        
        sequenceScale.Append(scale1);
        sequenceScale.Append(scale2);

        sequenceSlamHammer.Join(sequenceMove);
        sequenceSlamHammer.Join(sequenceScale);

        sequenceSlamHammer.Play();
    }
}
