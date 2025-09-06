using System.Collections.Generic;
using Resources.Scripts.JudgmentCreator.ScriptableObjects.DocumentsSO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public LogBookCreator logBookCreator;
    
    public List<Document> documentsUnlocked = new List<Document>();
    public List<StartJudgmentNode.Sentence> sentences = new List<StartJudgmentNode.Sentence>();

    public List<Document> documentsDialoguePlayed = new List<Document>();

    public EnvelopeDocument.TypeAcused finalAcused;
    public StartJudgmentNode.Sentence finalSentence;
    
    private void Awake()
    {
        instance = this;
    }

    public void AddDocumentToDialoguePlayed(Document doc)
    {
        if (documentsDialoguePlayed.Find(document => document == doc) != null) return;
        
        documentsDialoguePlayed.Add(doc);
    }

    public void AddUnlockedDocument(Document document)
    {
        documentsUnlocked.Add(document);
    }

    public bool IsDocumentUnlocked(Document document)
    {
        return documentsUnlocked.Find(it => it == document) != null;
    }
}
