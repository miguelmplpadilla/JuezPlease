using System.Collections;
using Resources.Scripts.SceneCreators.LogBookCreator;
using TMPro;
using UnityEngine;

public class ChargesCreator : MonoBehaviour
{
    public TextMeshProUGUI chargesText;
    public TextMeshProUGUI backgroundText;

    public GameObject prefabChargesSecondPage;
    
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
        
        ChargesNode node = s.node as ChargesNode;

        chargesText.text = node.acusations.value;
        backgroundText.text = node.chargesPages[0].value;

        for (int i = 1; i < node.chargesPages.Count; i++)
        {
            GameObject chargesPage = Instantiate(prefabChargesSecondPage, transform.parent);
            chargesPage.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
            chargesPage.transform.Find("Background").Find("TextBackground").GetComponent<TextMeshProUGUI>().text =
                node.chargesPages[i].value;
            chargesPage.transform.localScale = Vector3.zero;
            
            yield return new WaitForEndOfFrame();
        }
    }
}