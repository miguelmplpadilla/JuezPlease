using System;
using System.Collections.Generic;

[NodeWidth(400)]
public class DialogueNode : ConectionsNode
{
	public DialogController.TypeSpeaker speaker;
	public List<DialogueData> dialogues = new List<DialogueData>();
}

[Serializable]
public class DialogueData
{
	public List<Emotion> emotionsToPlay = new List<Emotion>();
	public LocalizableString text;
}

[Serializable]
public class Emotion
{
	public TypeArm arm;
	public TypeEmotion emotion;
	
	public enum TypeArm
	{
		LEFT, RIGHT
	}

	public enum TypeEmotion
	{
		POINT, PUNCH
	}
}