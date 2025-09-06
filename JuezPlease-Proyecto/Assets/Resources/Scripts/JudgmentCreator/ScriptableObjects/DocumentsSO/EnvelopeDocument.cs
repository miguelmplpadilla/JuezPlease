using UnityEngine;

namespace Resources.Scripts.JudgmentCreator.ScriptableObjects.DocumentsSO
{
    [CreateAssetMenu(fileName = "Envelope", menuName = "ScriptableObjects/Documents/Envelope", order = 1)]
    public class EnvelopeDocument : Document
    {
        public TypeAcused acused;
        
        public enum TypeAcused
        {
            NPCLEFT, NPCRIGHT
        }
    }
}