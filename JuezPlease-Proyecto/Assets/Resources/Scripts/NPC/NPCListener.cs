using UnityEngine;

public class SendDialogEvent : IEvent
{
    public Conversation conversation;
}

public class InteractNPCEvent : IEvent
{
    public GameObject obj;
    public GameObject objConversation;
}

public class LookToEvent : IEvent
{
    public bool lookToJudge = false;
    public GameObject objToLook;
}
