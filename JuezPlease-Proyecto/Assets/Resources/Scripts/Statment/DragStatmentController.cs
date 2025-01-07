public class DragStatmentController : DragObjectController
{
    protected override void Update()
    {
        base.Update();
        
        canJump = !transform.parent.name.Equals("ContinerStatments");
    }
}
