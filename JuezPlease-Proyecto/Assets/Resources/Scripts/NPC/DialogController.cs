using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
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
        foreach (var dialog in s.conversation.dialogs)
        {
            foreach (var text in dialog.textDialogs)
            {
                if (lastDialogObjs.Count > 0)
                {
                    foreach (var lastDialogObj in lastDialogObjs)
                    {
                        if (lastDialogObj.lastTypeSpeaker != dialog.typeSpeaker) continue;
                        float yLocalPosition = lastDialogObj.dialogObj.transform.localPosition.y;
                        lastDialogObj.dialogObj.transform.DOLocalMoveY(yLocalPosition + speakers[(int)dialog.typeSpeaker].sumY, 0.3f);
                    }
                }
                
                EventBus<LookToEvent>.Raise(new LookToEvent
                {
                    lookToJudge = dialog.typeSpeaker == Dialog.TypeSpeaker.JUDGE,
                    objToLook = speakers[(int)dialog.typeSpeaker].speakerObj
                });

                GameObject dialogObj = Instantiate(prefabDialog,
                    speakers[(int)dialog.typeSpeaker].speakerObj.transform);
                dialogObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
                dialogObj.transform.localScale = speakers[(int)dialog.typeSpeaker].scaleDialog * Vector3.one;
                dialogObj.transform.DOLocalMoveY(speakers[(int)dialog.typeSpeaker].sumYFirst, 0.3f);
                
                DialogObj dialogObjClass = new DialogObj
                {
                    dialogObj = dialogObj,
                    lastTypeSpeaker = dialog.typeSpeaker
                };
                StartCoroutine(DestroyDialogObj(dialogObjClass));
                lastDialogObjs.Add(dialogObjClass);
                
                yield return new WaitForSeconds(1.5f);
            }
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
public class Conversation
{
    public List<Dialog> dialogs;
}

[Serializable]
public class Dialog
{
    public enum TypeSpeaker
    {
        JUDGE, LAWYERLEFT, NPCLEFT, LAWYERRIGHT, NPCRIGHT
    }
    
    public TypeSpeaker typeSpeaker;
    [TextArea(3, 10)]
    public List<string> textDialogs = new List<string>();
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
    public Dialog.TypeSpeaker lastTypeSpeaker;
}
