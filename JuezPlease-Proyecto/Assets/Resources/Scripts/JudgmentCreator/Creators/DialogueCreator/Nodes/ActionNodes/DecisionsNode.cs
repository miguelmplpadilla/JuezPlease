
using XNode;

[CreateNodeMenu("DialogueCreator/Actions/Decisions")]
public class DecisionsNode : InputConectionNode
{
    [Output] public BaseNode decision1;
    public LocalizableString decisionText1;
    [Output] public BaseNode decision2;
    public LocalizableString decisionText2;
    [Output] public BaseNode decision3;
    public LocalizableString decisionText3;
    [Output] public BaseNode decision4;
    public LocalizableString decisionText4;
    
    public override void OnCreateConnection(NodePort from, NodePort to) {
		
        base.OnCreateConnection(from, to);

        if (ConnectDecision1(from, to)) return;
        if (ConnectDecision2(from, to)) return;
        if (ConnectDecision3(from, to)) return;
        if (ConnectDecision4(from, to)) return;
    }
    
    private bool ConnectDecision1(NodePort from, NodePort to)
    {
        DecisionsNode fromNode = from.node as DecisionsNode;
        BaseNode toNode = to.node as BaseNode;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "decision1")
        {
            fromNode.decision1 = toNode;
            return true;
        }

        return false;
    }

    private bool ConnectDecision2(NodePort from, NodePort to)
    {
        DecisionsNode fromNode = from.node as DecisionsNode;
        BaseNode toNode = to.node as BaseNode;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "decision2")
        {
            fromNode.decision2 = toNode;
            return true;
        }

        return false;
    }

    private bool ConnectDecision3(NodePort from, NodePort to)
    {
        DecisionsNode fromNode = from.node as DecisionsNode;
        BaseNode toNode = to.node as BaseNode;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "decision3")
        {
            fromNode.decision3 = toNode;
            return true;
        }

        return false;
    }

    private bool ConnectDecision4(NodePort from, NodePort to)
    {
        DecisionsNode fromNode = from.node as DecisionsNode;
        BaseNode toNode = to.node as BaseNode;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "decision4")
        {
            fromNode.decision4 = toNode;
            return true;
        }

        return false;
    }
    
    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("decision1")) decision1 = null;
        if (port.fieldName.Equals("decision2")) decision2 = null;
        if (port.fieldName.Equals("decision3")) decision3 = null;
        if (port.fieldName.Equals("decision4")) decision4 = null;
    }
}