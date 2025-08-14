using System;
using System.Collections.Generic;

public class EvidencesNode : RightPage
{
	public List<Evidence> evidences = new List<Evidence>();
}

[Serializable]
public class Evidence
{
	public bool isUnlocked = true;
	public LocalizableString textPage;
}
