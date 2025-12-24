using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CallWitnessDragController : DragObjectController
{
    public CharacterData witnessData;

    private List<GameObject> objectsTextIndicatorDrag = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        
        objectsTextIndicatorDrag.Add(GameObject.Find("PublicPanel"));
    }

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
            StartCoroutine(BenchController.instance.CallWitness(witnessData));

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

    protected override void GlobalDrag()
    {
        CheckIndicator(objectsTextIndicatorDrag);
    }
}
