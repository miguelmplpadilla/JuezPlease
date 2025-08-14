using System.Collections;
using UnityEngine;

public class DrawerResolutionController : DocumentCreator
{
    protected override IEnumerator AfterCreateDocument(RectTransform currentDocument)
    {
        yield return null;
        
        float xSize = (parentDocuments.rect.size.x / 2) - ((currentDocument.rect.x + 50) * currentDocument.localScale.x);
        float ySize = (parentDocuments.rect.size.y / 2) - ((currentDocument.rect.y + 50) * currentDocument.localScale.x);

        currentDocument.anchoredPosition = new Vector2(
            Random.Range(-xSize, xSize),
            Random.Range(-ySize, ySize));
    }
}
