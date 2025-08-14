using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DocumentCreator : MonoBehaviour
{
    public JudgmentCreator judgmentCreator;

    public GameObject photoPrefab;

    public RectTransform parentDocuments;

    public GameObject canvasObjs;
    
    private void Start()
    {
        foreach (var node in judgmentCreator.nodes)
        {
            if (node is AllStartDocuments allStartDocuments)
            {
                StartCoroutine(CreateDocumentsStart(allStartDocuments));
                break;
            }
        }
    }

    protected virtual IEnumerator CreateDocumentsStart(AllStartDocuments allStartDocuments)
    {
        yield return new WaitForSeconds(1);

        RectTransform currentDocumentCreated = null;

        foreach (var document in allStartDocuments.allDocumentsStart)
        {
            if (document is Photo photo) currentDocumentCreated = CreatePhoto(photo);

            yield return AfterCreateDocument(currentDocumentCreated);
        }
    }

    protected virtual IEnumerator AfterCreateDocument(RectTransform currentDocument)
    {
        currentDocument.anchoredPosition = new Vector2(parentDocuments.rect.size.x, Random.Range(-200, 200));

        yield return null;
            
        currentDocument.DOAnchorPosX(Random.Range(-50, 50), 1f);

        yield return new WaitForSeconds(0.4f);
    }

    protected RectTransform CreatePhoto(Photo photo)
    {
        RectTransform photoObject = Instantiate(photoPrefab, parentDocuments.transform).GetComponent<RectTransform>();
        DragObjectController dragObjectController = photoObject.GetComponent<DragObjectController>();
        dragObjectController.document = photo;
        dragObjectController.SetDataObject(parentDocuments.gameObject, parentDocuments.GetComponent<HolderController>(),
            !parentDocuments.tag.Equals("LittleObjects"));
        dragObjectController.canvas = canvasObjs;
        
        return photoObject;
    }
}