using System.Collections.Generic;
using UnityEngine;

public class Document : ScriptableObject
{
    public List<Dialog.TypeSpeaker> speakersAssigned = new List<Dialog.TypeSpeaker>();
    public DialogueCreator dialogue;
}