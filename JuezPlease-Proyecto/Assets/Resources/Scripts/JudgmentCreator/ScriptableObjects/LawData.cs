using UnityEngine;

namespace Resources.Scripts.JudgmentCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LawData", menuName = "ScriptableObjects/Law", order = 1)]
    public class LawData : ScriptableObject
    {
        public LocalizableString titleLaw;
        public LocalizableString descriptionLaw;

        public Vector2Int sentenceAmount;
        public Color colorPanelSentenceAmount;

        public StartJudgmentNode.Sentence sentence;
    }
}