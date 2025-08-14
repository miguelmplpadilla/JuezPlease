using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class NumbersStampDragController : DragObjectController
{
    protected override IEnumerator CheckIfIsInBlock()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            { position = transform.position };
        EventSystem.current.RaycastAll(pointerEventData, results);
        
        foreach (var obj in results)
        {
            if (obj.gameObject.name.Equals("DestroyNumbers"))
            {
                Destroy(gameObject);
                yield break;
            }
        }
        
        yield return base.CheckIfIsInBlock();
    }
}
