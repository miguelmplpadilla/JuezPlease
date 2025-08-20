using System.Collections.Generic;
using UnityEngine;

public class Document : ScriptableObject
{
    [HideInInspector] public string guid;
    
    public bool isUnlocked = false;
    
    public List<DialogueCreator> posibleDialogues;
    
    public void Awake()
    {
        guid = System.Guid.NewGuid().ToString();
    }
}