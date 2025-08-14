using System.Collections.Generic;
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

public class PlayAnimationNPCEvent : IEvent
{
    public GameObject obj;
    public List<Emotion> emotions = new List<Emotion>();
}
