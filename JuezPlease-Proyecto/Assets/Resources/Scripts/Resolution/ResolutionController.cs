using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    public RectTransform pressRt;
    public Image shadowImage;

    public Animator animatorLever;

    public GameObject continerStampsPaper;
    
    public GameObject continerTableStampeds;
    public GameObject continerPaperStampeds;
    
    public GameObject panelPress;
    public GameObject imagePress;
    
    public RectTransform paperShredder;

    public GameObject papersContiner;
    
    public RectTransform paperMaximumSentence;

    public RectTransform envelopeSentences;

    public RectTransform allTables;

    public CanvasGroup canvasGroup;
    public CanvasGroup canvasGroupEnvelope;

    public bool isTableShowed = false;

    private List<GameObject> stampedsPaper = new List<GameObject>();

    public GameObject objectsContiner;

    public List<RectTransform> boxesLidsLeft = new List<RectTransform>();
    public List<RectTransform> boxesLidsRight = new List<RectTransform>();

    public StateResolution stateResolution = StateResolution.MAXIMUMSENTENCE;

    public enum StateResolution
    {
        MAXIMUMSENTENCE, SENTENCES
    }

    private void Start()
    {
        EventBus<SelectSentencePaperEvent>.Register(new EventBinding<SelectSentencePaperEvent>(StartSelectSentencePaper, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<SelectSentencePaperEvent>.Deregister(new EventBinding<SelectSentencePaperEvent>(StartSelectSentencePaper, gameObject));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(ShowTable());
    }

    public void PlayShowTable()
    {
        StartCoroutine(ShowTable());
    }

    private IEnumerator ShowTable()
    {
        canvasGroup.blocksRaycasts = false;
        allTables.DOAnchorPosY(!isTableShowed ? Screen.height : 0, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1);
        canvasGroup.blocksRaycasts = true;
        isTableShowed = !isTableShowed;
    }

    public void PlayAnimationPress()
    {
        animatorLever.SetTrigger("down");
        StartCoroutine(AnimationPress());
    }

    private IEnumerator AnimationPress()
    {
        canvasGroup.blocksRaycasts = false;
        
        Vector2 originalPosition = pressRt.anchoredPosition;
        Vector3 originalScale = imagePress.transform.localScale;
        
        pressRt.DOAnchorPosY(198.4f, 0.2f);
        imagePress.transform.DOScale(1, 0.2f);

        shadowImage.transform.DOScale(1, 0.2f);
        shadowImage.DOFade(1, 0.2f);

        yield return new WaitForSeconds(0.2f);

        StampDragController stampDragController = null;

        stampedsPaper.Clear();
        
        while (continerStampsPaper.transform.childCount > 0)
        {
            stampDragController = continerStampsPaper.transform.GetChild(0).GetComponent<StampDragController>();
            GameObject prefabStamped = stampDragController.prefabStamped;

            stampedsPaper.Add(CreateStamp(prefabStamped, continerPaperStampeds,
                continerStampsPaper.transform.GetChild(0).position, stampDragController));
            CreateStamp(prefabStamped, continerTableStampeds,
                continerStampsPaper.transform.GetChild(0).position, stampDragController);
            
            continerStampsPaper.transform.GetChild(0).SetParent(panelPress.transform);
        }

        shadowImage.DOFade(0, 0);

        shadowImage.transform.DOShakePosition(0.1f, Vector3.one * 5);

        yield return new WaitForSeconds(0.5f);
        
        pressRt.DOAnchorPosY(originalPosition.y, 0.4f);
        imagePress.transform.DOScale(originalScale, 0.4f);

        if (stampedsPaper.Count == 0)
        {
            canvasGroup.blocksRaycasts = true;
            yield break;
        }
        
        yield return new WaitForSeconds(0.4f);

        if (stampedsPaper.Count > 1 && stateResolution == StateResolution.MAXIMUMSENTENCE)
        {
            yield return AnimationReturnMaximumSentencePaper();
            canvasGroup.blocksRaycasts = true;
            yield break;
        }

        StartCoroutine(HideAllStamps());
        if (stateResolution != StateResolution.MAXIMUMSENTENCE)
        {
            yield return SentencesPath();
            yield break;
        }

        yield return MaximumSentencePath(stampDragController as SentenceStampDragController);
    }

    private IEnumerator MaximumSentencePath(SentenceStampDragController sentenceStampDragController)
    {
        stateResolution = StateResolution.SENTENCES;
        
        yield return HidePaperMaximumSentence();

        if (sentenceStampDragController.typeStamp == SentenceStampDragController.TypeStamp.INOCENT)
        {
            GoToJudedScene();

            Debug.Log("Transition to JudgedScene");
            
            yield break;
        }
        
        if (sentenceStampDragController.typeStamp == SentenceStampDragController.TypeStamp.GUILTY)
            yield return ShowEnvelopeSentencesGuilty();
        else if (sentenceStampDragController.typeStamp == SentenceStampDragController.TypeStamp.INSANE)
            yield return ShowEnvelopeSentencesGuilty();
    }
    
    public IEnumerator SentencesPath()
    {
        stampedsPaper.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        string cantNumbers = "";
        for (int i = 0; i < stampedsPaper.Count; i++)
            cantNumbers += stampedsPaper[i].name;

        Debug.Log("Cant numbers: "+cantNumbers);

        StartCoroutine(AnimationPaperSentenceHide(papersContiner.transform.GetChild(0).gameObject));
        
        boxesLidsLeft[1].DOAnchorPosX(0, 2);
        boxesLidsRight[1].DOAnchorPosX(0, 2);

        yield return new WaitForSeconds(0.2f);
        
        boxesLidsLeft[0].DOAnchorPosX(0, 2);
        boxesLidsRight[0].DOAnchorPosX(0, 2);
        
        yield return new WaitForSeconds(3);
        
        GoToJudedScene();

        Debug.Log("Transition to JudgedScene");
    }

    private IEnumerator AnimationPaperSentenceHide(GameObject paper)
    {
        paper.transform.SetParent(GameObject.Find("CanvasTable").transform);

        paper.GetComponent<RectTransform>().DOAnchorPosY(1080, 1.5f);

        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator HideAllStamps()
    {
        SentenceStampDragController[] allStamps = FindObjectsOfType<SentenceStampDragController>();

        foreach (var stamp in allStamps)
        {
            if (!stamp.transform.parent.name.Equals("ObjectsContiner")) continue;

            stamp.rt.DOAnchorPosY(-Screen.height, 1);
        }

        yield return new WaitForSeconds(1);
        
        foreach (var stamp in allStamps)
            Destroy(stamp.gameObject);
    }

    private IEnumerator HidePaperMaximumSentence()
    {
        paperMaximumSentence.transform.SetParent(GameObject.Find("CanvasTable").transform);

        paperMaximumSentence.DOAnchorPosY(1080, 1.5f);

        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator ShowEnvelopeSentencesGuilty()
    {
        canvasGroupEnvelope.blocksRaycasts = true;
        
        envelopeSentences.DOAnchorPosY(0, 1);
        
        GameObject continerPapersEnvelope = envelopeSentences.transform.GetChild(1).gameObject;

        List<Vector2> originalPositions = new List<Vector2>();

        for (int i = 0; i < continerPapersEnvelope.transform.childCount; i++)
        {
            RectTransform rtPaper = continerPapersEnvelope.transform.GetChild(i).GetComponent<RectTransform>();
            originalPositions.Add(rtPaper.anchoredPosition);

            rtPaper.anchoredPosition -= new Vector2(0, 100);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < originalPositions.Count; i++)
        {
            RectTransform rtPaper = continerPapersEnvelope.transform.GetChild(i).GetComponent<RectTransform>();
            rtPaper.DOAnchorPosY(originalPositions[i].y, 1);
        }

        yield return new WaitForSeconds(1);
    }

    private IEnumerator AnimationReturnMaximumSentencePaper()
    {
        Transform originalParentPaper = paperMaximumSentence.transform.parent;
        
        paperMaximumSentence.transform.SetParent(objectsContiner.transform);

        yield return new WaitForEndOfFrame();

        paperShredder.DOAnchorPosX(0, 1);
        
        paperMaximumSentence.transform.DORotate(new Vector3(0, 0, 90), 1);
        paperMaximumSentence.transform.DOMoveY(paperShredder.transform.position.y, 1);
        yield return new WaitForSeconds(1.5f);

        paperMaximumSentence.transform.DOMoveX(paperShredder.transform.position.x, 1);

        yield return new WaitForSeconds(1);

        paperMaximumSentence.transform.DOMoveX(paperShredder.transform.GetChild(0).position.x, 3.5f);

        yield return new WaitForSeconds(4f);
        
        paperMaximumSentence.transform.SetParent(originalParentPaper);

        for (int i = 0; i < stampedsPaper.Count; i++)
            Destroy(stampedsPaper[i]);
        
        stampedsPaper.Clear();
            
        EventBus<ReturnToOriginalPositionEvent>.Raise(new ReturnToOriginalPositionEvent());
            
        paperMaximumSentence.transform.DORotate(Vector3.zero, 0);
        paperMaximumSentence.DOAnchorPos(new Vector3(0, -Screen.width, 0), 0);

        yield return new WaitForEndOfFrame();
            
        paperShredder.DOAnchorPosX(270, 1);
        paperMaximumSentence.DOAnchorPos(Vector3.zero, 2);

        yield return new WaitForSeconds(2);
    }

    private void StartSelectSentencePaper(SelectSentencePaperEvent s)
    {
        StartCoroutine(SelectSentencePaper(s));
    }

    private IEnumerator SelectSentencePaper(SelectSentencePaperEvent s)
    {
        envelopeSentences.DOAnchorPosY(-Screen.height, 1);
        
        continerStampsPaper = s.paperSentence.transform.Find("ContinerPaper").gameObject;
        if (continerStampsPaper != null) continerStampsPaper.GetComponent<Image>().raycastTarget = true;
        continerPaperStampeds = s.paperSentence.transform.Find("StampPaperContiner").gameObject;
        
        canvasGroup.blocksRaycasts = false;

        s.paperSentence.transform.SetParent(canvasGroupEnvelope.transform);
        s.paperSentence.transform.DOMove(papersContiner.transform.position, 1);

        if (s.sentence != StartJudgmentNode.Sentence.DEATH)
        {
            boxesLidsLeft[0].DOAnchorPosX(-450, 2);
            boxesLidsRight[0].DOAnchorPosX(450, 2);

            yield return new WaitForSeconds(0.2f);
        
            boxesLidsLeft[1].DOAnchorPosX(-450, 2);
            boxesLidsRight[1].DOAnchorPosX(450, 2);
        }

        yield return new WaitForSeconds(2);
        
        s.paperSentence.transform.SetParent(papersContiner.transform);
        
        if (s.sentence == StartJudgmentNode.Sentence.DEATH)
        {
            GoToJudedScene();
            
            yield break;
        }

        yield return new WaitForSeconds(1);
        
        canvasGroup.blocksRaycasts = true;
        canvasGroupEnvelope.blocksRaycasts = false;
    }

    private GameObject CreateStamp(GameObject prefab, GameObject parent, Vector3 position, StampDragController stampDragController)
    {
        GameObject stamped = Instantiate(prefab, parent.transform);
        stamped.transform.position = position;

        if (stampDragController is NumberStampDragController numberStamp)
        {
            stamped.GetComponent<Image>().sprite = numberStamp.spriteStamped;
            stamped.name = numberStamp.name;
        }

        return stamped;
    }

    private void GoToJudedScene()
    {
        EventBus<TransitionSceneEvent>.Raise(new TransitionSceneEvent
        {
            currentSceneName = "ResolutionScene",
            sceneNameToTransition = "JudgedScene",
            callback = () =>
            {
                EventBus<ReturnToJudgedEvent>.Raise(new ReturnToJudgedEvent());
            }
        });
    }
}

public class SelectSentencePaperEvent : IEvent
{
    public GameObject paperSentence;
    public StartJudgmentNode.Sentence sentence;
}
