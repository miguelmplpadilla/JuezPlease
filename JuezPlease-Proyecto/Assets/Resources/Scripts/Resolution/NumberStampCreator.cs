using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumberStampCreator : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite spriteStamp;
    public Sprite spriteStamped;
    public int numberStamp;
    
    public GameObject prefabStamp;
    private GameObject stampInstantiated;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(PointerDown(eventData));
    }

    private IEnumerator PointerDown(PointerEventData eventData)
    {
        GameObject parentStamp = GameObject.Find("ObjectsContiner");
        stampInstantiated = Instantiate(prefabStamp, GameObject.Find("ObjectsContiner").transform);
        NumberStampDragController stampDragController = stampInstantiated.GetComponent<NumberStampDragController>();
        
        stampInstantiated.transform.position = eventData.position;
        stampInstantiated.name = numberStamp.ToString();
        
        stampDragController.imageStamp.sprite = spriteStamp;
        stampDragController.spriteStamped = spriteStamped;
        stampDragController.canvas = parentStamp;

        yield return null;
        
        EventBus<OnBeginDragEvent>.Raise(new OnBeginDragEvent
        {
            obj = stampInstantiated,
            eventData = eventData
        });
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        EventBus<OnDragEvent>.Raise(new OnDragEvent
        {
            obj = stampInstantiated,
            eventData = eventData
        });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventBus<OnEndDragEvent>.Raise(new OnEndDragEvent
        {
            obj = stampInstantiated,
            eventData = eventData
        });
        stampInstantiated = null;
    }
}
