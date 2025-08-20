using System.Collections.Generic;
using UnityEngine;
using XNode;

public class StartJudgmentNode : OutputConectionNode 
{
    [Space(15)]
    [Output] public BaseNode startDocuments;
    [Output] public BaseNode startStatments;
    [Space(15)]
    
    [Tooltip("Sentencias para culpable desbloqueadas en este juicio")]
    public List<SentenceGuilty> sentencesGuilty = new List<SentenceGuilty>();
    [Tooltip("Sentencias para insane desbloqueadas en este juicio")]
    public List<SentenceInsane> sentencesInsane = new List<SentenceInsane>();

    public LogBookCreator logBook;

    public enum SentenceGuilty
    {
        NONE,
        DEATH,
        MONEY,
        WORK,
        PRISON
    }
    
    public enum SentenceInsane
    {
        NONE,
        PSYCHIATRIC,
        LOBOTOMY,
        ELECTROSHOCK,
        CHEMICALSTRAITJACKET
    }
    
    public override void OnCreateConnection(NodePort from, NodePort to) {
		
        base.OnCreateConnection(from, to);

        if (ConnectDocuments(from, to)) return;
        if (ConnectStatments(from, to)) return;
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

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("startDocuments")) startDocuments = null;
        if (port.fieldName.Equals("startStatments")) startStatments = null;
    }
}