using System;
using Resources.Scripts.JudgmentScene;
using UnityEngine;

public class Document : ScriptableObject
{
    [HideInInspector] public string guid;

    public DialogController.TypeSpeaker principalSpeaker;
    
    public bool isUnlocked = false;

    public bool hasStamp = false;
    
    public bool isFalse = false;
    public bool canBeFalse = false;
    public bool isMarkedHasFalse = false;
    
    public DialogueCreator dialogue;
    public DialogueFalse dialogueFalse;

    public int phoneNumber = -1;
    
    public MinistryDocumentsController.Ministry ministryDocument;
    
    public void Awake()
    {
        guid = Guid.NewGuid().ToString();
    }

    [Serializable]
    public class DialogueFalse
    {
        public LocalizableString textStatment;
        public DialogueCreator dialogue;
    }
}