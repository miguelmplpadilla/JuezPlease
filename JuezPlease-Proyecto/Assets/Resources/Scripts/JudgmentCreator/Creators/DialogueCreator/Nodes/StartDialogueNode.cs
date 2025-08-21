using XNode;

[CreateNodeMenu("DialogueCreator/StartDialogue")]
public class StartDialogueNode : BaseNode {

	[Output] public BaseNode npcLeftOutput;
	[Output] public BaseNode lawyerLeftOutput;
	[Output] public BaseNode npcRightOutput;
	[Output] public BaseNode lawyerRightOutput;
	
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);

		if (ConnectNPCLeft(from, to)) return;
		if (ConnectLawyerLeft(from, to)) return;
		if (ConnectNPCRight(from, to)) return;
		if (ConnectLawyerRight(from, to)) return;
	}

	private bool ConnectNPCLeft(NodePort from, NodePort to)
	{
		StartDialogueNode fromNode = from.node as StartDialogueNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "npcLeftOutput")
		{
			fromNode.npcLeftOutput = toNode;
			return true;
		}

		return false;
	}
	
	private bool ConnectNPCRight(NodePort from, NodePort to)
	{
		StartDialogueNode fromNode = from.node as StartDialogueNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "npcRightOutput")
		{
			fromNode.npcRightOutput = toNode;
			return true;
		}

		return false;
	}
	
	private bool ConnectLawyerLeft(NodePort from, NodePort to)
	{
		StartDialogueNode fromNode = from.node as StartDialogueNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "lawyerLeftOutput")
		{
			fromNode.lawyerLeftOutput = toNode;
			return true;
		}

		return false;
	}
	
	private bool ConnectLawyerRight(NodePort from, NodePort to)
	{
		StartDialogueNode fromNode = from.node as StartDialogueNode;
		BaseNode toNode = to.node as BaseNode;

		if (fromNode == null || toNode == null) return false;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "baseInput" && from.fieldName == "lawyerRightOutput")
		{
			fromNode.lawyerRightOutput = toNode;
			return true;
		}

		return false;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("npcLeftOutput")) npcLeftOutput = null;
		if (port.fieldName.Equals("lawyerLeftOutput")) lawyerLeftOutput = null;
		if (port.fieldName.Equals("npcRightOutput")) npcRightOutput = null;
		if (port.fieldName.Equals("lawyerRightOutput")) lawyerRightOutput = null;
	}

	public DialogueNode GetDialogueNodeBySpeaker(DialogController.TypeSpeaker speaker)
	{
		switch (speaker)
		{
			case DialogController.TypeSpeaker.NPCLEFT:
				return npcLeftOutput as DialogueNode;
			case DialogController.TypeSpeaker.LAWYERLEFT:
				return lawyerLeftOutput as DialogueNode;
			case DialogController.TypeSpeaker.NPCRIGHT:
				return npcRightOutput as DialogueNode;
			case DialogController.TypeSpeaker.LAWYERRIGHT:
				return lawyerRightOutput as DialogueNode;
			default:
				return null;
		}
	}
}