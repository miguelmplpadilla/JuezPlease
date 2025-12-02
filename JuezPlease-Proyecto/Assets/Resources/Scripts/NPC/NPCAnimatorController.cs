using UnityEngine;
using UnityEngine.UI;

public class NPCAnimatorController : MonoBehaviour
{
    public Sprite leftArmPointing;
    public Sprite leftArmPunch;
    
    public Sprite rightArmPointing;
    public Sprite rightArmPunch;

    public Image actionArmLeft;
    public Image actionArmRight;
    
    public void ChangeLeftArmToPoint()
    {
        actionArmLeft.sprite = leftArmPointing;
    }
    
    public void ChangeRightArmToPoint()
    {
        actionArmRight.sprite = rightArmPointing;
    }
    
    public void ChangeLeftArmToPunch()
    {
        actionArmLeft.sprite = leftArmPunch;
    }
    
    public void ChangeRightArmToPunch()
    {
        actionArmRight.sprite = rightArmPunch;
    }
}
