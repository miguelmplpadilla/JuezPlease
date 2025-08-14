using UnityEngine;
using UnityEngine.EventSystems;

public class OnBeginDragEvent : IEvent
{
    public GameObject obj;
    public PointerEventData eventData;
}

public class OnDragEvent : IEvent
{
    public GameObject obj;
    public PointerEventData eventData;
}

public class OnEndDragEvent : IEvent
{
    public GameObject obj;
    public PointerEventData eventData;
}