using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts.NPC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController instance;
    
    public enum TypeSpeaker
    {
        JUDGE, LAWYERLEFT, NPCLEFT, LAWYERRIGHT, NPCRIGHT, WITNESS, PHONE
    }
    
    public GameObject prefabDialog;
    public GameObject prefabDecision;
    
    private List<DialogObj> lastDialogObjs = new List<DialogObj>();

    public List<Speaker> speakers;

    public bool isSpeaking = false;
    private bool selectingDecision = false;

    public GameObject panelDecisions;

    public CanvasGroup panelBlock;
    
    private BaseNode baseNodeDialog;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EventBus<SendDialogEvent>.Register(new EventBinding<SendDialogEvent>(StartDialog, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<SendDialogEvent>.Deregister(new EventBinding<SendDialogEvent>(StartDialog, gameObject));
    }

    private void StartDialog(SendDialogEvent s)
    {
        if (!isSpeaking) StartCoroutine(ShowDialog(s));
    }

    private IEnumerator ShowDialog(SendDialogEvent s)
    {
        EventBus<HideButtonsCloseEvent>.Raise(new HideButtonsCloseEvent { hide = true });
        
        isSpeaking = true;
        
        if (s.document != null) 
            GameManager.instance.AddDocumentToDialoguePlayed(s.document);
        
        baseNodeDialog = s.dialogueStartNode;

        while (true)
        {
            yield return null;

            if (baseNodeDialog is ConectionsNode connectionNode)
            {
                if (connectionNode is DialogueNode dialogueNode)
                {
                    yield return PlayDialogue(dialogueNode);
                } else if (connectionNode is CallWitnessNode || connectionNode is HideWitnessNode) //TODO: Añadir sprites a el testigo
                {
                    yield return CallWitness(connectionNode is CallWitnessNode ? connectionNode as CallWitnessNode : null);
                } else if (connectionNode is UnlockDocumentNode unlockDocumentNode)
                {
                    UnlockDocument(unlockDocumentNode.documentToUnlock);
                } else if (connectionNode is HidePhone)
                {
                    TelephoneController.instance.HidePhone();
                } else if (connectionNode is CallPhoneNode)
                {
                    yield return CallPhone();
                }
            
                if (connectionNode == null || connectionNode.baseOutput == null) break;
            
                baseNodeDialog = connectionNode.baseOutput;
                
                continue;
            }
            
            if (baseNodeDialog is DecisionsNode decisionsNode)
            {
                yield return SelectDecision(decisionsNode);
            }
        }
        
        EventBus<HideButtonsCloseEvent>.Raise(new HideButtonsCloseEvent { hide = false });
        
        isSpeaking = false;
    }

    private IEnumerator PlayDialogue(DialogueNode dialogueNode)
    {
        foreach (var dialogue in dialogueNode.dialogues)
        {
            if (lastDialogObjs.Count > 0)
            {
                foreach (var lastDialogObj in lastDialogObjs)
                {
                    bool isInSameBench = CleanSpeakerName(lastDialogObj.lastTypeSpeaker)
                        .Equals(CleanSpeakerName(dialogueNode.speaker));
                    
                    if (lastDialogObj.lastTypeSpeaker != dialogueNode.speaker && !isInSameBench) continue;
                    float yLocalPosition = lastDialogObj.dialogObj.transform.localPosition.y;
                    lastDialogObj.dialogObj.transform.DOLocalMoveY(yLocalPosition + speakers[(int)dialogueNode.speaker].sumY, 0.3f);
                }
            }
        
            EventBus<LookToEvent>.Raise(new LookToEvent
            {
                lookToJudge = dialogueNode.speaker == TypeSpeaker.JUDGE,
                objToLook = speakers[(int)dialogueNode.speaker].speakerObj.transform.parent.gameObject
            });

            EventBus<PlayAnimationNPCEvent>.Raise(new PlayAnimationNPCEvent
            {
                obj = speakers[(int)dialogueNode.speaker].speakerObj.transform.parent.gameObject,
                emotions = dialogue.emotionsToPlay
            });

            GameObject dialogObj = Instantiate(prefabDialog,
                speakers[(int)dialogueNode.speaker].speakerObj.transform);
            dialogObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.text.value;
            dialogObj.transform.localScale = Vector3.zero;

            if (dialogueNode.speaker.Equals(TypeSpeaker.PHONE))
                dialogObj.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
            
            dialogObj.transform.DOScale(speakers[(int)dialogueNode.speaker].scaleDialog, 0.3f);
            dialogObj.transform.DOLocalMoveY(speakers[(int)dialogueNode.speaker].sumYFirst, 0.3f);
        
            DialogObj dialogObjClass = new DialogObj
            {
                dialogObj = dialogObj,
                lastTypeSpeaker = dialogueNode.speaker
            };
            StartCoroutine(DestroyDialogObj(dialogObjClass));
            lastDialogObjs.Add(dialogObjClass);
        
            yield return new WaitForSeconds(1.5f);
        }
    }

    private string CleanSpeakerName(TypeSpeaker speaker)
    {
        return speaker.ToString().Replace("NPC", "")
            .Replace("LAWYER", "");
    }

    private IEnumerator CallWitness(CallWitnessNode callWitnessNode)
    {
        if (callWitnessNode != null)
        {
            GameObject dialogParent = speakers[(int)TypeSpeaker.WITNESS].speakerObj;
            for (int i = 0; i < dialogParent.transform.childCount; i++)
                Destroy(dialogParent.transform.GetChild(i).gameObject);
            
            lastDialogObjs.Clear();
        }
        StartCoroutine(BenchController.instance.CallWitness(callWitnessNode.witnessData));
        yield return new WaitForSeconds(1);
    }

    private IEnumerator DestroyDialogObj(DialogObj dialog)
    {
        yield return new WaitForSeconds(3);
        
        if (dialog.dialogObj == null) yield break;

        dialog.dialogObj.GetComponent<CanvasGroup>().DOFade(0, 1);
        yield return new WaitForSeconds(1);

        lastDialogObjs.Remove(dialog);
        Destroy(dialog.dialogObj);
    }

    private void UnlockDocument(Document document)
    {
        if (GameManager.instance.IsDocumentUnlocked(document)) return;
        
        if (document is Statment statment) StatmentsController.instance.CreateNewStatment(statment);
        else if (document is Photo) StartCoroutine(DocumentCreator.instance.CreateDocument(document, 1));
        
        GameManager.instance.AddUnlockedDocument(document);
    }

    private IEnumerator CallPhone()
    {
        yield return TelephoneController.instance.ShowHideNPCAnimation(true);
        yield return new WaitForSeconds(0.5f);
        yield return TelephoneController.instance.AnimationCall();
    }

    private IEnumerator SelectDecision(DecisionsNode decisionsNode)
    {
        panelBlock.DOFade(0.4f, 0.5f);
        panelBlock.blocksRaycasts = true;
        
        selectingDecision = true;
        PlayDecisions(decisionsNode);
        while (selectingDecision) yield return null;
        
        panelBlock.DOFade(0, 0.5f);
        panelBlock.blocksRaycasts = false;
    }

    private void SetBaseDialogNode(BaseNode node)
    {
        baseNodeDialog = node;
        selectingDecision = false;
        for (int i = 0; i < panelDecisions.transform.childCount; i++)
            for (int j = 0; j < panelDecisions.transform.GetChild(i).childCount; j++)
                Destroy(panelDecisions.transform.GetChild(i).GetChild(j).gameObject);
    }

    private void PlayDecisions(DecisionsNode decisionsNode)
    {
        SetDecision(decisionsNode.decisionText1, panelDecisions.transform.GetChild(1), decisionsNode.decision1);
        SetDecision(decisionsNode.decisionText2, panelDecisions.transform.GetChild(1), decisionsNode.decision2);
        SetDecision(decisionsNode.decisionText3, panelDecisions.transform.GetChild(0), decisionsNode.decision3);
        SetDecision(decisionsNode.decisionText4, panelDecisions.transform.GetChild(0), decisionsNode.decision4);
        
        panelDecisions.transform.localScale = Vector3.one;
    }

    private void SetDecision(LocalizableString textDecision, Transform parent, BaseNode node)
    {
        if (node == null) return;
        GameObject decision = Instantiate(prefabDecision, parent);
        decision.GetComponent<Button>().onClick.AddListener(() => SetBaseDialogNode(node));
        decision.GetComponentInChildren<TextMeshProUGUI>().text = textDecision.value;
    }
}

[Serializable]
public class Speaker
{
    public GameObject speakerObj;
    public float scaleDialog;
    public float sumYFirst = 110;
    public float sumY = 75;
}

public class DialogObj
{
    public GameObject dialogObj;
    public DialogController.TypeSpeaker lastTypeSpeaker;
}

public class HideButtonsCloseEvent : IEvent
{
    public bool hide;
}