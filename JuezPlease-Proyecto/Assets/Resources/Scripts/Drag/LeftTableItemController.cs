using DG.Tweening;
using Resources.Scripts.Holder;
using UnityEngine;
namespace Resources.Scripts.Drag
{
    public class LeftTableItemController : DragObjectController
    {
        protected override void GlobalOnEndDrag()
        {
            if (currentParent.name.Equals("RightTableObjects") || currentParent.name.Equals("DrawerObjects") ||
                isAnimating) return;
        
            isAnimating = true;
        
            EventBus<SetObjectToListEvent>.Raise(new SetObjectToListEvent
            {
                obj = gameObject,
                objHolder = currentParent,
                add = true
            });
            
            transform.SetParent(originalParent.transform);
            transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(() =>
            {
                currentParent = null;
                isAnimating = false;
            });
        }
    }
}