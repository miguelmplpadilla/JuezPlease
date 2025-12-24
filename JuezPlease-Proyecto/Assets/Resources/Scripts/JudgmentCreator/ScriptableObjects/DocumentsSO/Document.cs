using UnityEngine;

public class Document : ScriptableObject
{
    [HideInInspector] public string guid;
    
    public bool isUnlocked = false;
    
    public DialogueCreator dialogue;

    public int phoneNumber = -1;
    
    public void Awake()
    {
        guid = System.Guid.NewGuid().ToString();
    }
}