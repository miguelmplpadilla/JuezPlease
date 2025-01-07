using UnityEngine;

public class SendDialogEvent : IEvent
{
    public Conversation conversation;
    public GameObject speaker;
    public float scaleDialog = 1;
}

public class InteractNPCEvent : IEvent
{
    public GameObject obj;
    public Conversation conversation;
}
