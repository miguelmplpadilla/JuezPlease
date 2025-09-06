using System;
using System.Collections;
using DG.Tweening;
using Resources.Scripts.JudgmentCreator.ScriptableObjects.DocumentsSO;
using UnityEngine;
using Random = UnityEngine.Random;

public class DocumentCreator : MonoBehaviour
{
    public static DocumentCreator instance;
    
    public GameObject photoPrefab;
    public GameObject envelopePrefab;

    public RectTransform parentDocuments;

    public RectTransform positionCreateObject;

    public bool isBig = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CreateDocumentsStart(JudgedSceneController.instance.allStartDocuments));
    }

    protected virtual IEnumerator CreateDocumentsStart(AllStartDocuments allStartDocuments)
    {
        yield return new WaitForSeconds(1);
        
        foreach (var document in allStartDocuments.allDocumentsStart)
            yield return CreateDocument(document);
    }

    public IEnumerator CreateDocument(Document document)
    {
        RectTransform currentDocumentCreated = null;

        float randomDiferenceY = 50;
        
        if (document is Photo photo) currentDocumentCreated = CreateDocumentObj(photo, photoPrefab);
        if (document is EnvelopeDocument envelope) 
        {
            currentDocumentCreated = CreateDocumentObj(envelope, envelopePrefab);
            randomDiferenceY = 0;
        }

        yield return AfterCreateDocument(currentDocumentCreated, randomDiferenceY);
    }

    protected virtual IEnumerator AfterCreateDocument(RectTransform currentDocument, float diference)
    {
        currentDocument.anchoredPosition = new Vector2(positionCreateObject.anchoredPosition.x, positionCreateObject.anchoredPosition.y + Random.Range(-200, 200));

        yield return null;
            
        currentDocument.DOAnchorPosX(Random.Range(-diference, diference), 1f);

        yield return new WaitForSeconds(0.4f);
    }

    protected RectTransform CreateDocumentObj(Document document, GameObject prefab)
    {
        RectTransform photoObject = Instantiate(prefab, parentDocuments.transform).GetComponent<RectTransform>();
        DragObjectController dragObjectController = photoObject.GetComponent<DragObjectController>();
        dragObjectController.document = document;
        dragObjectController.SetDataObject(parentDocuments.gameObject, parentDocuments.GetComponent<HolderController>(),
            isBig);
        dragObjectController.canvas = parentDocuments.gameObject;
        
        dragObjectController.SetVisualData(document);
        
        return photoObject;
    }
}