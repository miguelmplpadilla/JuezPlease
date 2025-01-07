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
                        if (lastDialogObj.numSpeaker != dialog.numSpeaker) continue;
                        float yLocalPosition = lastDialogObj.dialogObj.transform.localPosition.y;
                        lastDialogObj.dialogObj.transform.DOLocalMoveY(yLocalPosition + 75, 0.3f);
                    }
                }

                GameObject dialogObj = Instantiate(prefabDialog,
                    speakers[dialog.numSpeaker].speakerObj.transform);
                dialogObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
                dialogObj.transform.localScale = speakers[dialog.numSpeaker].scaleDialog * Vector3.one;
                dialogObj.transform.DOLocalMoveY(110, 0.3f);
                
                DialogObj dialogObjClass = new DialogObj
                {
                    dialogObj = dialogObj,
                    numSpeaker = dialog.numSpeaker
                };
                StartCoroutine(DestroyDialogObj(dialogObjClass));
                lastDialogObjs.Add(dialogObjClass);
                
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    private IEnumerator DestroyDialogObj(DialogObj dialog)
    {
        yield return new WaitForSeconds(5);
        
        yield return dialog.dialogObj.GetComponent<CanvasGroup>().DOFade(0, 1).AsyncWaitForCompletion();

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
    [Range(1, 4)] public int numSpeaker;
    public List<string> textDialogs = new List<string>();
}

[Serializable]
public class Speaker
{
    public GameObject speakerObj;
    public float scaleDialog;
}

public class DialogObj
{
    public GameObject dialogObj;
    public int numSpeaker;
}
