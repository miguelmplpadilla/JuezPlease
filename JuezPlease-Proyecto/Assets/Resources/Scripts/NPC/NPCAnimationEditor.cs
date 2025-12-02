using UnityEngine;
using UnityEngine.UI;

public class NPCAnimationEditor : MonoBehaviour
{
    public Button leftArmPointButton;
    public Button leftArmPunchButton;
    
    public Button rightArmPointButton;
    public Button rightArmPunchButton;

    public Animator animatorNpc;

    private void Start()
    {
        leftArmPointButton.onClick.AddListener(() => PlayAnimation("pointleft"));
        leftArmPunchButton.onClick.AddListener(() => PlayAnimation("punchleft"));
        
        rightArmPointButton.onClick.AddListener(() => PlayAnimation("pointright"));
        rightArmPunchButton.onClick.AddListener(() => PlayAnimation("punchright"));
    }

    private void PlayAnimation(string triggerAnimation)
    {
        animatorNpc.SetTrigger(triggerAnimation);
    }
}
