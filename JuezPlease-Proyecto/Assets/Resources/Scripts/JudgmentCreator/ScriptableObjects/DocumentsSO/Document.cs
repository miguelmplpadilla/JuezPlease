using System.Collections.Generic;
using UnityEngine;

public class Document : ScriptableObject
{
    public List<DialogController.TypeSpeaker> speakersAssigned = new List<DialogController.TypeSpeaker>();
    public List<DialogueCreator> posibleDialogues;
}