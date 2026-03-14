using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Resources.Scripts.Hammer;
using Resources.Scripts.Holder;
using Resources.Scripts.JudgmentScene;
using Resources.Scripts.NPC;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DragObjectController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Document document;
    
    public float multiplyScaleForBig = 5;

    protected float originalScale = 1;
    protected float scaleBig = 1;

    public RectTransform[] imagesRectTransform;
    public RectTransform[] shadowsRectTransform;

    public GameObject imagesLittle;
    public GameObject imagesBig;

    public GameObject allImages;
    [SerializeField] protected GameObject continer;
    protected GameObject currentParent;
    protected GameObject originalParent;
    public GameObject canvas;

    public RectTransform stampPosition;

    protected Vector3 startPositionDrag;

    protected bool isAnimating = false;
    protected bool isDragging = false;

    protected bool canJump = true;

    private Sequence sequenceSlamHammer;
    
    public string objTag = "BigObjects";

    private bool isInShreeder = false;
    public bool isCrushable = true;
    protected bool isBigObject = false;

    public LocalizableString textIndicator;

    protected NPCController npcControllerSelected;

    protected virtual void Awake()
    {
        if (textIndicator.value.Replace(" ", "").Equals(""))
            textIndicator = new LocalizableString("Preguntar", "Ask");
        
        originalParent = transform.parent.gameObject;
        
        originalScale = transform.localScale.x;
        scaleBig = originalScale * multiplyScaleForBig;
        
        GameObject parent = transform.parent.gameObject;
        SetDataObject(parent, parent.GetComponent<HolderController>(), !parent.CompareTag("LittleObjects"));
    }
    
    protected virtual void Start()
    {
        EventBus<SlamHammerAnimationEvent>.Register(
            new EventBinding<SlamHammerAnimationEvent>(HammerSlamJump, gameObject));
        
        EventBus<OnBeginDragEvent>.Register(
            new EventBinding<OnBeginDragEvent>(BeginDrag, gameObject));
        EventBus<OnDragEvent>.Register(
            new EventBinding<OnDragEvent>(Drag, gameObject));
        EventBus<OnEndDragEvent>.Register(
            new EventBinding<OnEndDragEvent>(EndDrag, gameObject));
        
        if (canvas == null) canvas = GameObject.Find("CanvasTable");

        if (stampPosition != null && document != null && document.ministryDocument != MinistryDocumentsController.Ministry.NONE)
            MinistryDocumentsController.instance.CreateStamp(document.ministryDocument, stampPosition.gameObject, !document.isFalse);
    }

    protected virtual void OnDestroy()
    {
        EventBus<SlamHammerAnimationEvent>.Deregister(
            new EventBinding<SlamHammerAnimationEvent>(HammerSlamJump, gameObject));
        
        EventBus<OnBeginDragEvent>.Deregister(
            new EventBinding<OnBeginDragEvent>(BeginDrag, gameObject));
        EventBus<OnDragEvent>.Deregister(
            new EventBinding<OnDragEvent>(Drag, gameObject));
        EventBus<OnEndDragEvent>.Deregister(
            new EventBinding<OnEndDragEvent>(EndDrag, gameObject));
    }

    protected virtual void Update()
    {
        if (!isAnimating && isDragging)
            SetData();
    }

    private void BeginDrag(OnBeginDragEvent onBeginDragEvent)
    {
        if (isAnimating || !onBeginDragEvent.obj.Equals(gameObject)) return;

        startPositionDrag = transform.position;
        
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
        
        if (!DialogController.instance.isSpeaking && document != null) CheckAllNPC();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag(new OnBeginDragEvent
        {
            eventData = eventData,
            obj = gameObject
        });
    }

    private void Drag(OnDragEvent onDragEvent)
    {
        if (isAnimating || !onDragEvent.obj.Equals(gameObject)) return;
        
        isDragging = true;
        transform.position = onDragEvent.eventData.position;

        if (!DialogController.instance.isSpeaking && document != null) CheckNPCDraged();
        
        GlobalDrag();
    }

    protected virtual void GlobalDrag()
    {
        if (document == null) return;
        
        NPCController[] allNPCController = FindObjectsOfType<NPCController>();
        List<GameObject> objNPCs = new List<GameObject>();

        foreach (var npcController in allNPCController)
        {
            if (GetConversation(npcController) != null)
                objNPCs.Add(npcController.gameObject);
        }
        
        CheckIndicator(objNPCs);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag(new OnDragEvent
        {
            eventData = eventData,
            obj = gameObject
        });
    }
    
    private void EndDrag(OnEndDragEvent onEndDragEvent)
    {
        if (!onEndDragEvent.obj.Equals(gameObject)) return;
        
        StartCoroutine(EndDragIE(onEndDragEvent.eventData));
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        EndDrag(new OnEndDragEvent
        {
            eventData = eventData,
            obj = gameObject
        });
    }

    private IEnumerator EndDragIE(PointerEventData eventData)
    {
        if (isAnimating) yield break;

        if (isInShreeder)
        {
            StartCoroutine(CrushPaper());
            yield break;
        }
        
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

        yield return CheckIfIsInBlock();
        
        isDragging = false;
        
        TextIndicatorMouseController.instance.ShowHideTextIndicator(false);
        
        GlobalOnEndDrag();
    }

    private IEnumerator CrushPaper()
    {
        isAnimating = true;
        
        GameObject startPosition = GameObject.Find("StartPositionShredderTableData");
        GameObject endPosition = GameObject.Find("EndPositionShredderTableData");

        transform.DOMoveY(startPosition.transform.position.y, 0.3f);

        yield return new WaitForSeconds(0.4f);
        
        transform.DOMoveX(startPosition.transform.position.x, 0.3f);
        
        yield return new WaitForSeconds(0.3f);
        
        transform.DOMoveX(endPosition.transform.position.x, 4);

        yield return new WaitForSeconds(4.1f);
        
        Destroy(gameObject);
    }

    protected virtual IEnumerator CheckIfIsInBlock()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            { position = transform.position };
        EventSystem.current.RaycastAll(pointerEventData, results);
        
        foreach (var obj in results)
        {
            if (obj.gameObject.CompareTag("BlockDrag"))
            {
                isAnimating = true;
                transform.DOMove(startPositionDrag, 0.3f);
                yield return new WaitForSeconds(0.3f);
                isAnimating = false;

                yield break;
            }
        }
    }
    
    protected void CheckNPCDraged()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            { position = transform.position };
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var obj in results)
        {
            if (obj.gameObject.TryGetComponent(out NPCController npcController))
            {
                if (!npcController.canInteract) return;
                
                npcControllerSelected = npcController;
                allImages.transform.localScale = Vector3.one * 1.4f;
                
                return;
            }
        }

        npcControllerSelected = null;
        
        allImages.transform.localScale = Vector3.one * 1.05f;
    }
    
    protected void CheckAllNPC()
    {
        NPCController[] allNPCController = FindObjectsOfType<NPCController>();
        
        foreach (var npcController in allNPCController)
        {
            if (GetConversation(npcController) == null) continue;
            
            StartCoroutine(npcController.ShowHideNPCAnimation(true));

            EventBus<AboveInteractNPCEvent>.Raise(new AboveInteractNPCEvent
            {
                obj = npcController.gameObject,
                canInteract = true
            });
        }
    }

    protected void CheckIndicator(List<GameObject> objectsCheck)
    {
        TextIndicatorMouseController.instance.ShowHideTextIndicator(false);
        
        if (textIndicator.value.Replace(" ", "").Equals("")) return;
        
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            { position = transform.position };
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var obj in results)
        {
            foreach (var objCheck in objectsCheck)
            {
                if (objCheck == obj.gameObject)
                {
                    TextIndicatorMouseController.instance.SetTextIndicator(textIndicator);
                }
            }
        }
    }
    
    protected void UnCheckAllNPC()
    {
        Debug.Log("UnCheckAllNPC");
        NPCController[] allNPCController = FindObjectsOfType<NPCController>();
        
        foreach (var npcController in allNPCController)
        {
            StartCoroutine(npcController.ShowHideNPCAnimation(false));
            EventBus<AboveInteractNPCEvent>.Raise(new AboveInteractNPCEvent
            {
                obj = npcController.gameObject,
                canInteract = false
            });
        }
    }
    
    protected BaseNode GetConversation(NPCController npcController)
    {
        List<BaseNode> filteredBySpeaker = new List<BaseNode>();

        var startDialogueNode = document.dialogue.nodes
            .OfType<StartDialogueNode>().FirstOrDefault();

        if (startDialogueNode.GetDialogueNodeBySpeaker(npcController.speaker) is BaseNode baseNode && baseNode != null)
            filteredBySpeaker.Add(baseNode);
        
        if (filteredBySpeaker.Count == 0) return null;

        return filteredBySpeaker[Random.Range(0, filteredBySpeaker.Count)];
    }

    protected virtual void GlobalOnEndDrag()
    {
        allImages.transform.localScale = Vector3.one * 1.05f;

        if (!currentParent.name.Equals("PublicPanel"))
        {
            UnCheckAllNPC();
            return;
        }
        
        isAnimating = true;
        transform.DOMoveY(GameObject.Find("YEndTable").transform.position.y, 0.2f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                isAnimating = false;
            }).OnUpdate(() =>
            {
                SetData();
            });

        if (!DialogController.instance.isSpeaking && npcControllerSelected != null && document != null)
        {
            if (npcControllerSelected is TelephoneController && document.phoneNumber > -1 && TelephoneController.instance.phoneNumberCalled != document.phoneNumber)
            {
                TelephoneController.instance.StartCall(document.phoneNumber);
            } else if (GetConversation(npcControllerSelected) is BaseNode nodeDialog)
            {
                EventBus<InteractNPCEvent>.Raise(new InteractNPCEvent
                {
                    obj = npcControllerSelected.gameObject,
                    dialogueNode = nodeDialog,
                    document = document
                });
            }
            
            CallbackEndDrag();
        }
        
        UnCheckAllNPC();
    }

    protected virtual void CallbackEndDrag()
    {
        
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
                break;
            }
        }

        if (isCrushable)
        {
            foreach (var result in results)
            {
                if (result.gameObject.name.Equals("ShredderCollider"))
                {
                    RotateToShredder();
                    return;
                }
            }

            if (!isInShreeder) return;
            
            allImages.transform.DOKill();
            allImages.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            isInShreeder = false;
        }
    }

    private void RotateToShredder()
    {
        if (isInShreeder) return;
        
        allImages.transform.DOKill();
        allImages.transform.DORotate(new Vector3(0, 0, 90), 0.5f);

        isInShreeder = true;
    }

    public virtual void SetDataObject(GameObject result, HolderController holderController, bool isBig)
    {
        if (result.gameObject.Equals(currentParent)) return;
        
        GameObject currentImages = isBig ? imagesBig : imagesLittle;
        isBigObject = isBig;
        
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

    public virtual void SetVisualData(Document document)
    {
        
    }
}
