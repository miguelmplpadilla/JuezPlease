using System.Collections.Generic;
using Resources.Scripts.JudgmentCreator.Creators.JudgmentCreator.Nodes;
using UnityEngine;
using XNode;

public class StartJudgmentNode : OutputConectionNode 
{
    [Space(15)]
    [Output] public BaseNode startDocuments;
    [Output] public BaseNode startStatments;
    [Space(15)]
    
    [Tooltip("Sentencias desbloqueadas en este juicio")]
    public List<Sentence> sentences = new List<Sentence>();

    public LogBookCreator logBook;
    
    [Tooltip("Dialogo que se ejecuta cuando vuelves de la resolución del juicio")]
    public DialogueCreator dialogueReturnToJudged;
    
    [Output] public CorrectSentenceNode correctSentenceOutput;

    public enum Sentence
    {
        NONE = 0,
        INNOCENT = 0,
        MONEY = 1,
        WORK = 2,
        PRISON = 3,
        DEATH = 4,
        PSYCHIATRIC = 1,
        LOBOTOMY = 2,
        ELECTROSHOCK = 3,
        CHEMICALSTRAITJACKET = 4
    }
    
    public override void OnCreateConnection(NodePort from, NodePort to) {
		
        base.OnCreateConnection(from, to);

        if (ConnectDocuments(from, to)) return;
        if (ConnectStatments(from, to)) return;
        if (ConnectCorrectSentence(from, to)) return;
    }

    private bool ConnectDocuments(NodePort from, NodePort to)
    {
        StartJudgmentNode fromNode = from.node as StartJudgmentNode;
        AllStartDocuments toNode = to.node as AllStartDocuments;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "startDocuments")
        {
            fromNode.startDocuments = toNode;
            return true;
        }

        return false;
    }
    
    private bool ConnectStatments(NodePort from, NodePort to)
    {
        StartJudgmentNode fromNode = from.node as StartJudgmentNode;
        AllStartStatments toNode = to.node as AllStartStatments;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "startStatments")
        {
            fromNode.startStatments = toNode;
            return true;
        }

        return false;
    }
    
    private bool ConnectCorrectSentence(NodePort from, NodePort to)
    {
        StartJudgmentNode fromNode = from.node as StartJudgmentNode;
        CorrectSentenceNode toNode = to.node as CorrectSentenceNode;
        
        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "correctSentenceOutput")
        {
            fromNode.correctSentenceOutput = toNode;
            return true;
        }

        return false;
    }

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("startDocuments")) startDocuments = null;
        if (port.fieldName.Equals("startStatments")) startStatments = null;
        if (port.fieldName.Equals("correctSentenceOutput")) correctSentenceOutput = null;
    }
}