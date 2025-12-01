using System.Collections.Generic;
using Resources.Scripts.JudgmentCreator.ScriptableObjects;

[CreateNodeMenu("JudgmentCreator/LawBook")]
public class LawBookNode : InputConectionNode
{
    public List<LawData> estLaws = new List<LawData>();
    public List<LawData> civLaws = new List<LawData>();
    public List<LawData> socLaws = new List<LawData>();
    public List<LawData> ecoLaws = new List<LawData>();
    public List<LawData> grvLaws = new List<LawData>();
}
