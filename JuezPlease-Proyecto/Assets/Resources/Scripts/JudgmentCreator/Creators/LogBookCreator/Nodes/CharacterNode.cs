public class CharacterNode : InputConectionNode 
{
	public CharacterType characterType;
	public CharacterData characterData;
	
	public enum CharacterType
	{
		Client,
		Lawyer
	}
}