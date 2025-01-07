using UnityEngine;

namespace Resources.Scripts.Holder
{
    public class SetObjectToListEvent : IEvent
    {
        public GameObject objHolder;
        public GameObject obj;
        public bool add;
    }
}