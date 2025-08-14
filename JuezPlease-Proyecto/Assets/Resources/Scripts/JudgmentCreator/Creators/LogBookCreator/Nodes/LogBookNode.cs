using System.Collections.Generic;
using XNode;

public class LogBookNode : BaseNode
{
    [Output] public BaseNode acusedOutput;
    [Output] public BaseNode acusatorOutput;
    
    [Output] public OutputConectionNode startRightPages;
	
    public override void OnCreateConnection(NodePort from, NodePort to) {
		
        base.OnCreateConnection(from, to);

        bool isConnected = false;
		
        isConnected = ConnectAcusedAcusator(from, to);
        if (isConnected) return;
        isConnected = ConnectStartRightPages(from, to);
        if (isConnected) return;
        
        DisconectAllFromConnections(from);
    }

    private bool ConnectAcusedAcusator(NodePort from, NodePort to)
    {
        LogBookNode fromNode = from.node as LogBookNode;
        LeftPage toNode = to.node as LeftPage;

        if (fromNode == null || toNode == null) return false;

        if (from.GetConnections().Count > 1)
            DisconectAllFromConnections(from);

        if (to.fieldName == "baseInput")
        {
            if (from.fieldName == "acusedOutput")
            {
                fromNode.acusedOutput = toNode;
                return true;
            }
            else if (from.fieldName == "acusatorOutput")
            {
                fromNode.acusatorOutput = toNode;
                return true;
            }
        }

        return false;
    }

    private bool ConnectStartRightPages(NodePort from, NodePort to)
    {
        LogBookNode fromNode = from.node as LogBookNode;
        RightPage toNode = to.node as RightPage;

        if (fromNode == null || toNode == null)
        {
            DisconectAllFromConnections(from);
            return false;
        }

        if (from.GetConnections().Count > 1)
            DisconectAllFromConnections(from);

        if (to.fieldName == "baseInput" && from.fieldName == "startRightPages")
        {
            fromNode.startRightPages = toNode;
            return true;
        }
        
        return false;
    }

    private void DisconectAllFromConnections(NodePort from)
    {
        for (int i = 0; i < from.GetConnections().Count; i++)
            from.Disconnect(i);
    }

    public override void OnRemoveConnection(NodePort port)
    {
        if (port.fieldName.Equals("acusedOutput")) acusedOutput = null;
        if (port.fieldName.Equals("acusatorOutput")) acusatorOutput = null;
    }
}