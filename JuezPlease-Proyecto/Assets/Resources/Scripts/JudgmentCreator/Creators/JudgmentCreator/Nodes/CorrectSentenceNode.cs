using Resources.Scripts.JudgmentCreator.ScriptableObjects.DocumentsSO;
using UnityEngine;
using XNode;

namespace Resources.Scripts.JudgmentCreator.Creators.JudgmentCreator.Nodes
{
    [CreateNodeMenu("DialogueCreator/CorrectSentence")]
    public class CorrectSentenceNode : InputConectionNode
    {
        public int minCantQuestionsToAsk = 3;
        [Tooltip("Sentencia correcta en el juicio")]
        public StartJudgmentNode.Sentence sentence;
        [Tooltip("Cantidad de años, dinero o dias que tendra la sentencia (No aplicable a muerte)")]
        public int cantSentence = 5;
        [Tooltip("Acusado correcto del juicio")]
        public EnvelopeDocument.TypeAcused correctAcused;
        
        [Output] public StartDialogueEmotionsNode envelopeDialoguesNpcLeft;
        [Output] public StartDialogueEmotionsNode envelopeDialoguesNpcRight;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
		
            base.OnCreateConnection(from, to);

            if (ConnectNpcLeftDialogues(from, to)) return;
            if (ConnectNpcRightDialogues(from, to)) return;
        }
        
        private bool ConnectNpcLeftDialogues(NodePort from, NodePort to)
        {
            CorrectSentenceNode fromNode = from.node as CorrectSentenceNode;
            StartDialogueEmotionsNode toNode = to.node as StartDialogueEmotionsNode;
        
            if (fromNode == null || toNode == null) return false;

            if (from.GetConnections().Count > 1)
                for (int i = 0; i < from.GetConnections().Count; i++)
                    from.Disconnect(i);

            if (to.fieldName == "baseInput" && from.fieldName == "envelopeDialoguesNpcLeft")
            {
                fromNode.envelopeDialoguesNpcLeft = toNode;
                return true;
            }

            return false;
        }
    
        private bool ConnectNpcRightDialogues(NodePort from, NodePort to)
        {
            CorrectSentenceNode fromNode = from.node as CorrectSentenceNode;
            StartDialogueEmotionsNode toNode = to.node as StartDialogueEmotionsNode;
        
            if (fromNode == null || toNode == null) return false;

            if (from.GetConnections().Count > 1)
                for (int i = 0; i < from.GetConnections().Count; i++)
                    from.Disconnect(i);

            if (to.fieldName == "baseInput" && from.fieldName == "envelopeDialoguesNpcRight")
            {
                fromNode.envelopeDialoguesNpcRight = toNode;
                return true;
            }

            return false;
        }
        
        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);

            if (port.fieldName.Equals("envelopeDialoguesNpcRight")) envelopeDialoguesNpcRight = null;
            if (port.fieldName.Equals("envelopeDialoguesNpcLeft")) envelopeDialoguesNpcLeft = null;
        }
        
        public StartDialogueEmotionsNode GetDialogueByAcused(EnvelopeDocument.TypeAcused acused)
        {
            if (acused == EnvelopeDocument.TypeAcused.NPCLEFT)
                return envelopeDialoguesNpcLeft;
            if (acused == EnvelopeDocument.TypeAcused.NPCRIGHT)
                return envelopeDialoguesNpcRight;
            return null;
        }
    }
}