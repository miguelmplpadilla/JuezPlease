using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CharactersNode : InputConectionNode 
{
	[Space(15)]
	[Output] public BaseNode clientOutput;
	[Output] public BaseNode lawyerOutput;
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);
		
		CharactersNode fromNode = from.node as CharactersNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput")
		{
			if (from.fieldName == "clientOutput") fromNode.clientOutput = toNode;
			else if (from.fieldName == "lawyerOutput") fromNode.lawyerOutput = toNode;
		}
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("clientOutput")) clientOutput = null;
		if (port.fieldName.Equals("lawyerOutput")) lawyerOutput = null;
	}
}