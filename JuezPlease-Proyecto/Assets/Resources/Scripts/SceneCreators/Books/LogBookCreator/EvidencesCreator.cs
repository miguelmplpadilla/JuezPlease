using System.Collections;
using Resources.Scripts.SceneCreators.LogBookCreator;
using TMPro;
using UnityEngine;

public class EvidencesCreator : MonoBehaviour
{
    public TextMeshProUGUI evidencesText;

    public GameObject prefabEvidences;
    
    private void Awake()
    {
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Register(
            new EventBinding<LogBookSceneCreatorListener.SetNodeDataEvent>(SetDataChargesPage, gameObject));
    }
    
    private void OnDestroy()
    {
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Deregister(
            new EventBinding<LogBookSceneCreatorListener.SetNodeDataEvent>(SetDataChargesPage, gameObject));
    }

    private void SetDataChargesPage(LogBookSceneCreatorListener.SetNodeDataEvent s)
    {
        StartCoroutine(SetData(s));
    }
    
    private IEnumerator SetData(LogBookSceneCreatorListener.SetNodeDataEvent s)
    {
        if (!s.obj.Equals(gameObject)) yield break;

        yield return new WaitForEndOfFrame();
        
        EvidencesNode node = s.node as EvidencesNode;

        evidencesText.text = node.evidences[0].textPage.value;
        
        if (node.evidences[0].isUnlocked) transform.localScale = Vector3.zero;

        for (int i = 1; i < node.evidences.Count; i++)
        {
            if (!node.evidences[i].isUnlocked) continue;
            
            GameObject evidencesPage = Instantiate(prefabEvidences, transform.parent);
            evidencesPage.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
            evidencesPage.transform.Find("TextEvidences").GetComponent<TextMeshProUGUI>().text =
                node.evidences[i].textPage.value;
            evidencesPage.transform.localScale = Vector3.zero;
            
            yield return new WaitForEndOfFrame();
        }
    }
}