using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.SceneCreators.LogBookCreator;
using TMPro;
using UnityEngine;

public class LogBookSceneCreator : MonoBehaviour
{
    private LogBookNode logBookNode = null;

    public GameObject leftContiner;
    public GameObject rightContiner;

    public GameObject acusedAcusatorPrefab;
    
    public GameObject chargesPrefab;

    public GameObject evidencesPrefab;
    public GameObject witnessPrefab;

    public int indexLeft = 0;
    public int indexRight = 0;

    public TextMeshProUGUI numPagesText;

    private void Start()
    {
        FindLogBookNode();
        if (logBookNode == null)
        {
            Debug.LogError("No se ha detectado el LogBookNode en el creador, sin este node no se puede crear en libro");
            return;
        }
        StartCoroutine(CreateLogBook());
    }

    public IEnumerator CreateLogBook()
    {
        yield return CreateAcusedAcusator();
        
        OutputConectionNode nextNode = logBookNode.startRightPages;
        
        List<GameObject> objCreated = new List<GameObject>();
        
        while (true)
        {
            GameObject chargesObj = Instantiate(GetPrefabByType(nextNode),
                nextNode is LeftPage ? leftContiner.transform : rightContiner.transform);
            objCreated.Add(chargesObj);
            
            chargesObj.transform.localScale = Vector3.zero;
            
            if (nextNode.baseOutput == null) break;

            nextNode = nextNode.baseOutput as OutputConectionNode;
        }
        
        leftContiner.transform.GetChild(0).localScale = Vector3.one;
        rightContiner.transform.GetChild(0).localScale = Vector3.one;

        yield return new WaitForEndOfFrame();
        
        nextNode = logBookNode.startRightPages;

        for (int i = 0; i < objCreated.Count; i++)
        {
            EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Raise(new LogBookSceneCreatorListener.SetNodeDataEvent
            {
                obj = objCreated[i],
                node = nextNode
            });
            
            if (nextNode.baseOutput == null) break;

            nextNode = nextNode.baseOutput as OutputConectionNode;
        }

        yield return new WaitForEndOfFrame();

        SetNumPageText();
    }

    private void FindLogBookNode()
    {
        foreach (var node in GameManager.instance.logBookCreator.nodes)
        {
            if (node is LogBookNode)
            {
                logBookNode = node as LogBookNode;
                break;
            }
        }
    }

    private IEnumerator CreateAcusedAcusator()
    {
        GameObject acusedObj = Instantiate(acusedAcusatorPrefab, leftContiner.transform);
        acusedObj.transform.localScale = Vector3.zero;
        GameObject acusatorObj = Instantiate(acusedAcusatorPrefab, leftContiner.transform);
        acusatorObj.transform.localScale = Vector3.zero;
        
        yield return new WaitForEndOfFrame();
        
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Raise(new LogBookSceneCreatorListener.SetNodeDataEvent
        {
            obj = acusatorObj,
            node = logBookNode.acusatorOutput
        });
        EventBus<LogBookSceneCreatorListener.SetNodeDataEvent>.Raise(new LogBookSceneCreatorListener.SetNodeDataEvent
        {
            obj = acusedObj,
            node = logBookNode.acusedOutput
        });
        
        yield return new WaitForEndOfFrame();
    }
    
    private GameObject GetPrefabByType(BaseNode type)
    {
        if (type is ChargesNode) return chargesPrefab;
        if (type is EvidencesNode) return evidencesPrefab;
        if (type is WitnessNode) return witnessPrefab;

        return null;
    }

    public void RestartBook()
    {
        for (int i = 0; i < leftContiner.transform.childCount; i++)
            Destroy(leftContiner.transform.GetChild(i).gameObject);
        for (int i = 0; i < rightContiner.transform.childCount; i++)
            Destroy(rightContiner.transform.GetChild(i).gameObject);

        StartCoroutine(CreateLogBook());

        indexLeft = 0;
        indexRight = 0;
        
        SetNumPageText();
    }

    public void ChangePageLeft(int sum)
    {
        if (indexLeft + sum < 0 || indexLeft + sum >= leftContiner.transform.childCount) return;
        
        indexLeft += sum;

        for (int i = 0; i < leftContiner.transform.childCount; i++)
            leftContiner.transform.GetChild(i).localScale = Vector3.zero;
        
        leftContiner.transform.GetChild(indexLeft).localScale = Vector3.one;
    }
    
    public void ChangePageRight(int sum)
    {
        if (indexRight + sum < 0 || indexRight + sum >= rightContiner.transform.childCount) return;
        
        indexRight += sum;
        
        for (int i = 0; i < rightContiner.transform.childCount; i++)
            rightContiner.transform.GetChild(i).localScale = Vector3.zero;
        
        rightContiner.transform.GetChild(indexRight).localScale = Vector3.one;
        
        SetNumPageText();
    }

    private void SetNumPageText()
    {
        numPagesText.text = (indexRight + 1) + "/" + rightContiner.transform.childCount;
    }
}
