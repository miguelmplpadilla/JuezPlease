using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public Sprite body;
    public Sprite eyes;
    public Sprite eyelids;
    public Arm armRight;
    public Arm armLeft;
}

[Serializable]
public class Arm
{
    public Sprite normalArm;
    public Sprite fistArm;
    public Sprite pointArm;
}