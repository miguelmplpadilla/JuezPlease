using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SentencePaperController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public StartJudgmentNode.Sentence sentence;
    
    private RectTransform rt;

    private Vector2 originalPosition;

    public GameObject continerStamps;

    private bool canInteract = true;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        originalPosition = rt.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canInteract) return;
        
        rt.DOKill();
        rt.DOAnchorPosY(originalPosition.y + 50, 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        canInteract = false;
        
        if (continerStamps != null) continerStamps.SetActive(true);
        
        rt.DOKill();
        
        EventBus<SelectSentencePaperEvent>.Raise(new SelectSentencePaperEvent
        {
            paperSentence = gameObject,
            sentence = sentence
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!canInteract) return;

        rt.DOKill();
        rt.DOAnchorPosY(originalPosition.y, 0.5f);
    }
    
    public void SetPosition(Vector2 position)
    {
        rt.anchoredPosition = position;
        originalPosition = position;
    }
}
