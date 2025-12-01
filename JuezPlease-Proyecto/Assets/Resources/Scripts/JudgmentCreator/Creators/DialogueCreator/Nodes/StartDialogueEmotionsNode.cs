using UnityEngine;
using XNode;

[CreateNodeMenu("DialogueCreator/StartDialogueEmotionsNode")]
public class StartDialogueEmotionsNode : InputConectionNode {

	[Tooltip("Este dialogo se ejcuta si la condena ha sido aceptable")]
	[Output] public BaseNode normalDialogueOutput;
	[Tooltip("Este dialogo se ejcuta si la condena ha sido mala")]
	[Output] public BaseNode angryDialogueOutput;
	[Tooltip("Este dialogo se ejcuta si la condena ha sido inocente")]
	[Output] public BaseNode innocentDialogueOutput;
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (ConnectNormalDialogue(from, to)) return;
		if (ConnectAngryDialogue(from, to)) return;
		if (ConnectInnocentDialogue(from, to)) return;
	}

	private bool ConnectNormalDialogue(NodePort from, NodePort to)
	{
		StartDialogueEmotionsNode fromNode = from.node as StartDialogueEmotionsNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "normalDialogueOutput")
		{
			fromNode.normalDialogueOutput = toNode;
			return true;
		}

		return false;
	}
	
	private bool ConnectAngryDialogue(NodePort from, NodePort to)
	{
		StartDialogueEmotionsNode fromNode = from.node as StartDialogueEmotionsNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "angryDialogueOutput")
		{
			fromNode.angryDialogueOutput = toNode;
			return true;
		}

		return false;
	}
	
	private bool ConnectInnocentDialogue(NodePort from, NodePort to)
	{
		StartDialogueEmotionsNode fromNode = from.node as StartDialogueEmotionsNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "innocentDialogueOutput")
		{
			fromNode.innocentDialogueOutput = toNode;
			return true;
		}

		return false;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("normalDialogueOutput")) normalDialogueOutput = null;
		if (port.fieldName.Equals("angryDialogueOutput")) angryDialogueOutput = null;
	}
}