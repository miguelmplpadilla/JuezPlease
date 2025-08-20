using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CharacterNode : InputConectionNode 
{
	public CharacterType characterType;
	
	public CharacterSprites characterSprites;
	
	public enum CharacterType
	{
		Client,
		Lawyer
	}
}

[Serializable]
public class CharacterSprites
{
	public Sprite body;
	public Sprite eyes;
	public Sprite eyelids;
	public Arm armRight;
	public Arm amrLeft;
}

[Serializable]
public class Arm
{
	public Sprite normalArm;
	public Sprite actionArm;
}