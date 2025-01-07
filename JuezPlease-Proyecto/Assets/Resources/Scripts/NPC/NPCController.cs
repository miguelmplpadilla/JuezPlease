using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float scaleDialog = 0.8f;
    public GameObject parentDialog;
    
    private void Start()
    {
        EventBus<InteractNPCEvent>.Register(new EventBinding<InteractNPCEvent>(Interact, gameObject));
    }
    
    private void OnDestroy()
    {
        EventBus<InteractNPCEvent>.Deregister(new EventBinding<InteractNPCEvent>(Interact, gameObject));
    }

    private void Interact(InteractNPCEvent i)
    {
        if (!i.obj.Equals(gameObject)) return;
        
        EventBus<SendDialogEvent>.Raise(new SendDialogEvent
        {
            conversation = i.conversation,
            speaker = parentDialog,
            scaleDialog = scaleDialog
        });
    }
}
