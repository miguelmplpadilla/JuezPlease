using System.Collections.Generic;
using UnityEngine;

public class SendDialogEvent : IEvent
{
    public DialogueNode dialogueStartNode;
    public DialogController.TypeSpeaker speaker;
}

public class AboveInteractNPCEvent : IEvent
{
    public GameObject obj;
    public bool canInteract = false;
}

public class InteractNPCEvent : IEvent
{
    public GameObject obj;
    public DialogueNode dialogueNode;
}

public class LookToEvent : IEvent
{
    public bool lookToJudge = false;
    public GameObject objToLook;
}

public class PlayAnimationNPCEvent : IEvent
{
    public GameObject obj;
    public List<Emotion> emotions = new List<Emotion>();
}
