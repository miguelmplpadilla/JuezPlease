public class ModifyReputationNode : ConectionsNode
{
    public TypeFaction faction;
    public float cantReputation;

    public enum TypeFaction
    {
        NONE, FACTION1, FACTION2, FACTION3, FACTION4
    }
}