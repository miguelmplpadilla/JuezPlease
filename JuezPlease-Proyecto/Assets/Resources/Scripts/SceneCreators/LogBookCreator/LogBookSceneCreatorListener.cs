using UnityEngine;

namespace Resources.Scripts.SceneCreators.LogBookCreator
{
    public class LogBookSceneCreatorListener
    {
        public class SetNodeDataEvent : IEvent
        {
            public GameObject obj;
            public BaseNode node;
        }
    }
}