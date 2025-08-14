using UnityEngine;
using XNode;

[CreateNodeMenu("")]
public class ConectionsNode : OutputConectionNode
{
    [Space(15)]
    [Input] public BaseNode baseInput;

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("baseInput")) baseInput = null;
        if (port.fieldName.Equals("baseOutput")) baseOutput = null;
    }
}