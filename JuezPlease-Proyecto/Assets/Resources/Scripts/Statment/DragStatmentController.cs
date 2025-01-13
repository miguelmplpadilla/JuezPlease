using TMPro;

public class DragStatmentController : DragObjectController
{
    public TextMeshProUGUI textStatment;
    public TextMeshProUGUI textNumber;
    
    protected override void Update()
    {
        base.Update();
        
        canJump = !transform.parent.name.Equals("ContinerStatments");
    }
}
