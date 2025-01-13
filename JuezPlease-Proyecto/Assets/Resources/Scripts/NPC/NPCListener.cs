using UnityEngine;

public class SendDialogEvent : IEvent
{
    public DialogueCreator dialogue;
}

public class InteractNPCEvent : IEvent
{
    public GameObject obj;
    public DialogueCreator dialogue;
}

public class LookToEvent : IEvent
{
    public bool lookToJudge = false;
    public GameObject objToLook;
}
