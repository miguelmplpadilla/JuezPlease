using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CallWitnessDragController : DragObjectController
{
    public CharacterData witnessData;

    public override void SetDataObject(GameObject result, HolderController holderController, bool isBig)
    {
        if (result.gameObject.Equals(currentParent)) return;
        
        GameObject currentImages = isBig ? imagesBig : imagesLittle;
        isBigObject = isBig;
        
        imagesBig.transform.localScale = Vector3.zero;
        imagesLittle.transform.localScale = Vector3.zero;
        currentImages.transform.localScale = Vector3.one;
        
        currentParent = result;
                
        continer = holderController.objContiner;
    }

    protected override void GlobalOnEndDrag()
    {
        if (currentParent.name.Equals("PublicPanel"))
        {
            StartCoroutine(BenchController.instance.CallWitness(witnessData));
        }

        StartCoroutine(ReturnToParent());
    }

    private IEnumerator ReturnToParent()
    {
        imagesBig.transform.localScale = Vector3.zero;
        imagesLittle.transform.localScale = Vector3.one;
        
        transform.SetParent(originalParent.transform);
        transform.DOLocalMove(Vector3.zero, 0.6f);
        yield return new WaitForSeconds(0.6f);
    }
}
