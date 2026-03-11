using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatmentsController : MonoBehaviour
{
    public static StatmentsController instance;
    
    public GameObject statmentPrefab;
    
    public bool isAnimating = false;
    public bool isShowed = false;
    
    public RectTransform continer;
    public RectTransform continerStatments;

    public Button dropDownButton;

    public RectTransform cantNewStatmentsIndicator;

    private int cantStatmentsCreated = 0;
    
    public List<DialogueCreator> notFalseDialogues = new List<DialogueCreator>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        List<Statment> startStatments =
            new List<Statment>(JudgedSceneController.instance.allStartStatments.allstatmentsStart);
        
        for (int i = 0; i < startStatments.Count; i++)
            CreateNewStatment(startStatments[i]);
        
        dropDownButton.onClick.AddListener(() =>
        { ShowStatments(); });
    }

    public void ShowStatments()
    {
        if (isAnimating) return;
        
        float yFinalPosition = 0;
        if (isShowed) yFinalPosition = 433;

        isAnimating = true;

        continer.DOAnchorPosY(yFinalPosition, 0.5f).OnComplete(() =>
        {
            dropDownButton.transform.localScale = new Vector3(1, -dropDownButton.transform.localScale.y, 1);
            isAnimating = false;
        }).SetEase(Ease.InOutBack);

        if (!isShowed) continerStatments.anchoredPosition = new Vector2(continerStatments.anchoredPosition.x, 0);
        
        cantNewStatmentsIndicator.DOAnchorPosY(-45, 0.6f).SetEase(Ease.OutBack);
        
        isShowed = !isShowed;
    }

    public void CreateNewStatment(Statment statment)
    {
        cantStatmentsCreated++;
        
        GameObject statmentObject = Instantiate(statmentPrefab, continerStatments.transform);
        statmentObject.transform.SetAsFirstSibling();
            
        DragStatmentController dragStatmentController = statmentObject.GetComponent<DragStatmentController>();
        dragStatmentController.document = statment;
            
        dragStatmentController.textStatment.text = statment.textStatment.value;
        dragStatmentController.textNumber.text = (cantStatmentsCreated).ToString();

        cantNewStatmentsIndicator.DOAnchorPosY(0, 0.6f).SetEase(Ease.InOutBack);
    }

    public DialogueCreator GetNotFalseDialogue(DialogController.TypeSpeaker speaker)
    {
        DialogueCreator dialogue = notFalseDialogues[Random.Range(0, notFalseDialogues.Count)];

        StartDialogueNode startDialogue = dialogue.nodes.Find(it => it is StartDialogueNode) as StartDialogueNode;
        DialogueNode dialogueNode = startDialogue.GetDialogueNodeBySpeaker(speaker) as DialogueNode;

        startDialogue.lawyerLeftOutput = null;
        startDialogue.lawyerRightOutput = null;
        startDialogue.npcLeftOutput = null;
        startDialogue.npcRightOutput = null;
        startDialogue.telephoneOutput = null;
        startDialogue.witnessNodeOutput = null;
        
        startDialogue.SetDialogueNodeBySpeaker(speaker, dialogueNode);

        DialogueNode nextDialogue = dialogueNode;

        while (nextDialogue != null)
        {
            if (nextDialogue.speaker != DialogController.TypeSpeaker.JUDGE)
                nextDialogue.speaker = speaker;
            
            nextDialogue = nextDialogue.baseOutput as DialogueNode;
        }

        return dialogue;
    }
}
