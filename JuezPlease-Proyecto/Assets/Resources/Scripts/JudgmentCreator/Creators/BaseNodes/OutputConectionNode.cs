using UnityEngine;
using XNode;

[CreateNodeMenu("")]
public class OutputConectionNode : BaseNode
{
    [Space(15)]
    [Output] public BaseNode baseOutput;
	
    public override void OnCreateConnection(NodePort from, NodePort to) {
		
        base.OnCreateConnection(from, to);
		
        OutputConectionNode fromNode = from.node as OutputConectionNode;
        BaseNode toNode = to.node as BaseNode;

        if (fromNode == null || toNode == null) return;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "baseInput" && from.fieldName == "baseOutput")
            fromNode.baseOutput = toNode;
    }

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("baseOutput")) baseOutput = null;
    }
}