using System;

[NodeWidth(438)]
public class CharacterNode : InputConectionNode 
{
	public CharacterData characterData;

	public TypeArmAnimation armAnimationToShowLeft = TypeArmAnimation.NORMAL;
	public TypeArmAnimation armAnimationToShowRight = TypeArmAnimation.NORMAL;

	public enum TypeArmAnimation
	{
		NORMAL, POINT
	}

	public bool IsDataVisualNotSeted()
	{
		try
		{
			return characterData.body == null && characterData.eyes == null && characterData.armRight.normalArm == null &&
			       characterData.armLeft.normalArm == null;
		} catch(Exception e) {}

		return true;
	}

	public void RestartData()
	{
		armAnimationToShowLeft = TypeArmAnimation.NORMAL;
		armAnimationToShowRight = TypeArmAnimation.NORMAL;

		characterData = new CharacterData();
	}
}