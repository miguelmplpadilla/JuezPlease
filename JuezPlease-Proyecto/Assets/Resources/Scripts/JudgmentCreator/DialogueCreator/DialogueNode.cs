using System;
using System.Collections.Generic;

[NodeWidth(400)]
public class DialogueNode : BaseNode
{
	public Dialog.TypeSpeaker speaker;
	public List<DialogueData> dialogues = new List<DialogueData>();
}

[Serializable]
public class DialogueData
{
	public TypeEmotion emotion;
	public LocalizableString text;

	public enum TypeEmotion
	{
		NORMAL, ANGRY, SAD, HAPPY
	}
}