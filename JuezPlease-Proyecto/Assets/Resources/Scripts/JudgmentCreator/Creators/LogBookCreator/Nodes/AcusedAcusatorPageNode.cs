using System;
using UnityEngine;

public class AcusedAcusatorPageNode : LeftPage
{
	public AcusedAcusatorData data;
}

[Serializable]
public class AcusedAcusatorData
{
	public Sprite photo;
	public bool isAcused = false;
	public LocalizableString litigantName;
	public LocalizableString surnames;
	public Gender gender;
	public string birthDate;
	public LocalizableString profession;
	public LocalizableString criminalRecord;
	public Sprite fingerPrint;
}

public enum Gender
{
	MALE, FEMALE
}