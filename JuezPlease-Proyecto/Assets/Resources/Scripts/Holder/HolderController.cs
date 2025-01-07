using System;
using System.Collections.Generic;
using Resources.Scripts.Holder;
using UnityEngine;

public class HolderController : MonoBehaviour
{
    public List<GameObject> objectsInContiner = new List<GameObject>();

    public GameObject objContiner;

    private void Start()
    {
        if (objContiner == null)
            objContiner = GameObject.Find("ObjectsContiner");
        
        EventBus<SetObjectToListEvent>.Register(new EventBinding<SetObjectToListEvent>(SetObjectList, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<SetObjectToListEvent>.Deregister(new EventBinding<SetObjectToListEvent>(SetObjectList, gameObject));
    }

    private void SetObjectList(SetObjectToListEvent s)
    {
        if (!s.objHolder.Equals(gameObject)) return;
        
        if (s.add)
        {
            objectsInContiner.Add(s.obj);
            return;
        }
        
        objectsInContiner.Remove(s.obj);
    }
}
