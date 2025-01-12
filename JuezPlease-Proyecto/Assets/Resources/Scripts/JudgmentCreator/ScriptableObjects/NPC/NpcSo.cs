using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC/NPC", order = 1)]
public class NpcSo : ScriptableObject
{
    public string npcName;
    public bool isAccused = true;
    public string professionalPosition;
    public string criminalRecord;
    public float levelCriminalRecord;
    public string dateBirth;
    public ModifyReputationNode.TypeFaction faction;
    public bool isGuilty = true;
    public TypeCrime crime;
    public float crimeLevel;
    public List<DialogueData.TypeEmotion> emotions;
    
    public NPCImages npcImages;

    public enum TypeCrime
    {
        NONE
    }
}
