using System;
using UnityEngine;

public class JudgedSceneController : MonoBehaviour
{
   public static JudgedSceneController instance;
   
   public JudgmentCreator judgmentCreator;

   [NonSerialized] public AllStartStatments allStartStatments;

   private void Awake()
   {
      instance = this;
      
      foreach (var node in judgmentCreator.nodes)
      {
         if (node is StartJudgmentNode startJudgmentNode)
         {
            allStartStatments = startJudgmentNode.startStatments as AllStartStatments;
            return;
         }
      }
   }
}
