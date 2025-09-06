public class EnvelopeDragController : DragObjectController
{
    protected override void CallbackEndDrag()
    {
        Destroy(gameObject);
    }
}
