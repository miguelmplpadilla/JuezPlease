using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public enum TypeSpeaker
    {
        JUDGE, LAWYERLEFT, NPCLEFT, LAWYERRIGHT, NPCRIGHT
    }
    
    public GameObject prefabDialog;
    
    private List<DialogObj> lastDialogObjs = new List<DialogObj>();

    public List<Speaker> speakers;
    
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
        StartCoroutine(ShowDialog(s));
    }

    private IEnumerator ShowDialog(SendDialogEvent s)
    {
        DialogueNode dialogueNode = null;
        foreach (var node in s.dialogue.nodes)
        {
            if (node is StartDialogueNode)
            {
                dialogueNode = (node as StartDialogueNode).baseOutput as DialogueNode;
                break;
            }
        }

        while (true)
        {
            yield return null;
            
            foreach (var dialogue in dialogueNode.dialogues)
            {
                if (lastDialogObjs.Count > 0)
                {
                    foreach (var lastDialogObj in lastDialogObjs)
                    {
                        if (lastDialogObj.lastTypeSpeaker != dialogueNode.speaker) continue;
                        float yLocalPosition = lastDialogObj.dialogObj.transform.localPosition.y;
                        lastDialogObj.dialogObj.transform.DOLocalMoveY(yLocalPosition + speakers[(int)dialogueNode.speaker].sumY, 0.3f);
                    }
                }
            
                EventBus<LookToEvent>.Raise(new LookToEvent
                {
                    lookToJudge = dialogueNode.speaker == TypeSpeaker.JUDGE,
                    objToLook = speakers[(int)dialogueNode.speaker].speakerObj
                });

                GameObject dialogObj = Instantiate(prefabDialog,
                    speakers[(int)dialogueNode.speaker].speakerObj.transform);
                dialogObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.text.value;
                dialogObj.transform.localScale = speakers[(int)dialogueNode.speaker].scaleDialog * Vector3.one;
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
            
            if (dialogueNode.baseOutput is EndDialogueNode) break;
            
            dialogueNode = dialogueNode.baseOutput as DialogueNode;
        }
    }

    private IEnumerator DestroyDialogObj(DialogObj dialog)
    {
        yield return new WaitForSeconds(3);

        dialog.dialogObj.GetComponent<CanvasGroup>().DOFade(0, 1);
        yield return new WaitForSeconds(1);

        lastDialogObjs.Remove(dialog);
        Destroy(dialog.dialogObj);
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
