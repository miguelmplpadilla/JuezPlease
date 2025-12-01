using System.Collections;
using DG.Tweening;
using Resources.Scripts.JudgmentCreator.ScriptableObjects.DocumentsSO;
using Resources.Scripts.SceneCreators.LogBookCreator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcusedSelectorButtonController : MonoBehaviour
{
    public Button buttonLeftAcused;
    public Button buttonRightAcused;

    public GameObject acusedAcusatorPageLeft;
    public GameObject acusedAcusatorPageRight;

    public GameObject stamp;
    public GameObject stamped;

    private LogBookNode logBookNode;

    private bool isSelected = false;

    public RectTransform panel;
    public CanvasGroup canvasGroup;
    
    public TextMeshProUGUI nameAcused;
    public TextMeshProUGUI nameAcusator;
    
    public TextMeshProUGUI nameAcusedPaper;

    private void Start()
    {
        FindLogBookNode();

        buttonLeftAcused.onClick.AddListener(() => StartCoroutine(SelectAcused(buttonLeftAcused.animator,
            logBookNode.acusatorOutput as AcusedAcusatorPageNode, logBookNode.acusedOutput as AcusedAcusatorPageNode,
            EnvelopeDocument.TypeAcused.NPCLEFT)));
        buttonRightAcused.onClick.AddListener(() => StartCoroutine(SelectAcused(buttonRightAcused.animator,
            logBookNode.acusedOutput as AcusedAcusatorPageNode, logBookNode.acusatorOutput as AcusedAcusatorPageNode,
            EnvelopeDocument.TypeAcused.NPCRIGHT)));
        
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Raise(new LogBookSceneCreatorListener.SetNodeDataEvent
        {
            obj = acusedAcusatorPageLeft,
            node = logBookNode.acusatorOutput
        });
        
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Raise(new LogBookSceneCreatorListener.SetNodeDataEvent
        {
            obj = acusedAcusatorPageRight,
            node = logBookNode.acusedOutput
        });
    }

    private IEnumerator SelectAcused(Animator animatorButton, AcusedAcusatorPageNode acusedNode, AcusedAcusatorPageNode acusatorNode, EnvelopeDocument.TypeAcused acused)
    {
        if (isSelected) yield break;
        isSelected = true;

        GameManager.instance.finalAcused = acused;
        
        nameAcused.text = acusedNode.data.litigantName.value + " " + acusedNode.data.surnames.value;
        nameAcusator.text = acusatorNode.data.litigantName.value + " " + acusatorNode.data.surnames.value;
        
        nameAcusedPaper.text = acusedNode.data.litigantName.value + " " + acusedNode.data.surnames.value;
        
        Vector3 positionMouse = Input.mousePosition;
        
        animatorButton.SetBool("stamped", true);
        Vector3 originalPosition = new Vector3(positionMouse.x, stamp.transform.position.y,
            stamp.transform.position.z);
        stamp.transform.position = originalPosition;
        
        yield return null;

        stamp.transform.DOScale(1, 0.2f).SetEase(Ease.Linear);
        stamp.transform.DOMoveY(positionMouse.y, 0.2f).SetEase(Ease.Linear);
        
        yield return new WaitForSeconds(0.4f);

        stamped.transform.position = stamp.transform.GetChild(0).position;
        stamped.SetActive(true);

        yield return null;
        
        stamp.transform.DOScale(1.2f, 0.2f).SetEase(Ease.Linear);
        stamp.transform.DOMove(originalPosition, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.5f);

        panel.DOAnchorPosY(Screen.height, 0.5f);
        yield return new WaitForSeconds(0.5f);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    
    private void FindLogBookNode()
    {
        logBookNode = GameManager.instance.logBookCreator.nodes.Find(it => it is LogBookNode) as LogBookNode;
    }
}
