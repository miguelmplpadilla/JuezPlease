using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    public List<ConversationNPC> conversationsNPC = new List<ConversationNPC>();

    public Image imageNPC;

    public Sprite spriteNoBlink;
    public Sprite spriteBlink;

    public RectTransform eyesRt;
    
    private void Start()
    {
        EventBus<InteractNPCEvent>.Register(new EventBinding<InteractNPCEvent>(Interact, gameObject));
        EventBus<LookToEvent>.Register(new EventBinding<LookToEvent>(LookTo, gameObject));
        
        if (spriteBlink != null) StartCoroutine(Blink());
    }
    
    private void OnDestroy()
    {
        EventBus<InteractNPCEvent>.Deregister(new EventBinding<InteractNPCEvent>(Interact, gameObject));
        EventBus<LookToEvent>.Deregister(new EventBinding<LookToEvent>(LookTo, gameObject));
    }

    private void Interact(InteractNPCEvent i)
    {
        if (!i.obj.Equals(gameObject)) return;
        
        EventBus<SendDialogEvent>.Raise(new SendDialogEvent
        {
            conversation = GetConversation(i.objConversation)
        });
    }

    private Conversation GetConversation(GameObject objConversation)
    {
        foreach (var conversation in conversationsNPC)
            if (conversation.objConversation.Equals(objConversation))
                return conversation.conversations[UnityEngine.Random.Range(0, conversation.conversations.Count)];

        return null;
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return null;
            yield return new WaitForSeconds(Random.Range(2, 10 + 1));
            imageNPC.sprite = spriteBlink;
            yield return new WaitForSeconds(0.05f);
            imageNPC.sprite = spriteNoBlink;
        }
    }

    private void LookTo(LookToEvent l)
    {
        if (l.lookToJudge)
        {
            eyesRt.DOAnchorPosX(10, 0);
            return;
        }

        eyesRt.DOAnchorPosX(l.objToLook.transform.position.x > transform.position.x ? 12 : 7.4f, 0);
    }
}

[Serializable]
public class ConversationNPC
{
    public GameObject objConversation;
    public List<Conversation> conversations = new List<Conversation>();
}
