using System;
using System.Collections.Generic;
using Resources.Scripts.JudgmentCreator.Creators.JudgmentCreator.Nodes;
using Resources.Scripts.JudgmentCreator.ScriptableObjects.DocumentsSO;
using UnityEngine;

public class JudgedSceneController : MonoBehaviour
{
   public static JudgedSceneController instance;
   
   public JudgmentCreator judgmentCreator;
   [NonSerialized] public StartJudgmentNode startJudgmentNode;

   [NonSerialized] public AllStartStatments allStartStatments;
   [NonSerialized] public AllStartDocuments allStartDocuments;

   public EnvelopeDocument.TypeAcused realAcused;
   
   public int cantQuestionsAsked = 0;

   private void Awake()
   {
      instance = this;
      
      foreach (var node in judgmentCreator.nodes)
      {
         if (node is StartJudgmentNode startNode)
         {
            startJudgmentNode = startNode;
            allStartStatments = startNode.startStatments as AllStartStatments;
            allStartDocuments = startNode.startDocuments as AllStartDocuments;
            break;
         }
      }
      
      GameManager.instance.startJudgmentNode = startJudgmentNode;
      
      GameManager.instance.sentences = new List<StartJudgmentNode.Sentence>(startJudgmentNode.sentences);
      GameManager.instance.logBookCreator = (judgmentCreator.nodes.Find(it => it is StartJudgmentNode) as StartJudgmentNode).logBook;
   }

   private void Start()
   {
      EventBus<ReturnToJudgedEvent>.Register(new EventBinding<ReturnToJudgedEvent>(ReturnToJudged, gameObject));
   }

   private void OnDestroy()
   {
      EventBus<ReturnToJudgedEvent>.Deregister(new EventBinding<ReturnToJudgedEvent>(ReturnToJudged, gameObject));
   }

   private void ReturnToJudged()
   {
      CreateEnvelope();
      
      if (startJudgmentNode.dialogueReturnToJudged == null) return;

      var correctSentenceNode = startJudgmentNode.dialogueReturnToJudged.nodes.Find(node => node is CorrectSentenceNode) as CorrectSentenceNode;
      if (correctSentenceNode == null) return;

      bool isCorrect = true;
      BaseNode finalDialogueSentence = null;
      if (GameManager.instance.finalSentence != StartJudgmentNode.Sentence.INNOCENT)
      {
         isCorrect = (GameManager.instance.finalAcused == correctSentenceNode.correctAcused) && GameManager.instance.finalSentence != correctSentenceNode.sentence;
         
         var dialogueByAcused = correctSentenceNode.GetDialogueByAcused(GameManager.instance.finalAcused);
         
         finalDialogueSentence = (isCorrect ? dialogueByAcused.normalDialogueOutput : dialogueByAcused.angryDialogueOutput);
         
         if (GameManager.instance.finalAcused != correctSentenceNode.correctAcused)
         {
            
         }
      }
      
      /*EventBus<SendDialogEvent>.Raise(new SendDialogEvent
      {
         dialogueStartNode =
            GameManager.instance.documentsDialoguePlayed.Count >= startJudgmentNode.correctSentenceOutput.minCantQuestionsToAsk
               ? startDialogue.normalDialogueOutput
               : startDialogue.angryDialogueOutput
      });*/
   }

   private void CreateEnvelope()
   {
      EnvelopeDocument envelopeDocument = new EnvelopeDocument();
      envelopeDocument.acused = GameManager.instance.finalAcused;

      DialogueCreator finalDialogue = new DialogueCreator();
      StartDialogueNode startDialogueNode = finalDialogue.AddNode(typeof(StartDialogueNode)) as StartDialogueNode;
      if (startDialogueNode == null) return;

      CorrectSentenceNode correctSentenceNode = startJudgmentNode.correctSentenceOutput;

      BaseNode nodeDialogue = GetCorrectEnvelopeDialogue(
         GameManager.instance.finalAcused == EnvelopeDocument.TypeAcused.NPCLEFT
            ? correctSentenceNode.envelopeDialoguesNpcLeft
            : correctSentenceNode.envelopeDialoguesNpcRight);

      if (GameManager.instance.finalAcused == EnvelopeDocument.TypeAcused.NPCLEFT)
      {
         startDialogueNode.npcLeftOutput = nodeDialogue;
         startDialogueNode.lawyerLeftOutput = nodeDialogue;
      } else if (GameManager.instance.finalAcused == EnvelopeDocument.TypeAcused.NPCRIGHT)
      {
         startDialogueNode.npcRightOutput = nodeDialogue;
         startDialogueNode.lawyerRightOutput = nodeDialogue;
      }

      envelopeDocument.dialogue = finalDialogue;

      StartCoroutine(DocumentCreator.instance.CreateDocument(envelopeDocument, 1));
   }

   private BaseNode GetCorrectEnvelopeDialogue(StartDialogueEmotionsNode startDialogueNode)
   {
      BaseNode nodeDialogue = IsFinalSentenceCorrect()
         ? startDialogueNode.normalDialogueOutput
         : startDialogueNode.angryDialogueOutput;
         
      if (GameManager.instance.finalSentence == StartJudgmentNode.Sentence.INNOCENT)
         nodeDialogue = startDialogueNode.innocentDialogueOutput;

      return nodeDialogue;
   }

   private bool IsFinalSentenceCorrect()
   {
      if (GameManager.instance.finalSentence > startJudgmentNode.correctSentenceOutput.sentence || 
          (GameManager.instance.finalSentence == startJudgmentNode.correctSentenceOutput.sentence && 
           GameManager.instance.numFinalSentence > startJudgmentNode.correctSentenceOutput.maxCantSentence) ||
          cantQuestionsAsked < startJudgmentNode.correctSentenceOutput.minCantQuestionsToAsk || 
          GameManager.instance.finalAcused != startJudgmentNode.correctSentenceOutput.correctAcused)
      {
         return false;
      }
      
      return true;
   }
}

public class ReturnToJudgedEvent : IEvent {}
