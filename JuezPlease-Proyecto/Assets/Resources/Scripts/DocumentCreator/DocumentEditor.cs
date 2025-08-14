using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DocumentEditor : MonoBehaviour
{
    public Document documentNode;
    public List<GameObject> extraDataObjectsCreated = new List<GameObject>();

    public GameObject photoPrefab;

    public TMP_FontAsset defaultFontAsset;

    private GameObject currentObjInstantiated;

    public Button buttonSave;

    public TextAsset extraDataDocuments;

    private void Start()
    {
        buttonSave.onClick.AddListener(SaveExtraDataPhoto);
        CreatePhoto(documentNode as Photo);
    }

    private void CreatePhoto(Photo photoNode)
    {
        currentObjInstantiated = Instantiate(photoPrefab, GameObject.Find("CanvasTable").transform);

        GameObject photo = FindObjectInChildren(currentObjInstantiated, "PhotoPanel");
        Image backgroundPhoto = FindObjectInChildren(photo, "PhotoBackground").GetComponent<Image>();
        Image photoImage = FindObjectInChildren(photo, "PhotoImage").GetComponent<Image>();
        
        backgroundPhoto.sprite = photoNode.backgroundImage;
        photoImage.sprite = photoNode.imagePhoto;
        
        CreateExtraData(photo, photoNode.extraDataDocument, photoNode.guid);
    }

    private void CreateExtraData(GameObject parent, ExtraDataDocument extraDataDocument, string guid)
    {
        ExtraDataDocument extraDataRtDocument = GetExtraDataDocument(guid);
        
        for (int i = 0; i < extraDataDocument.extraData.Count; i++)
        {
            var data = extraDataDocument.extraData[i];
            
            GameObject newObj = new GameObject();
            
            if (!data.text.dataText.value.Equals(""))
            {
                TextMeshProUGUI textMesh = newObj.AddComponent<TextMeshProUGUI>();
                textMesh.text = data.text.dataText.value;
                textMesh.font = data.text.fontText != null ? data.text.fontText : defaultFontAsset;
            }
            
            if (data.sprite != null)
            {
                Image textMesh = newObj.AddComponent<Image>();
                textMesh.sprite = data.sprite;
            }
            
            newObj.transform.SetParent(parent.transform);
            newObj.name = "ExtraData" + (i + 1);

            RectTransform rtNewObj = newObj.GetComponent<RectTransform>();
            rtNewObj.anchoredPosition = Vector2.zero;

            if (extraDataRtDocument != null)
            {
                newObj.transform.SetSiblingIndex(extraDataRtDocument.extraData[i].siblingIndex);
                SetRectTransformData(rtNewObj, extraDataRtDocument.extraData[i].rtData);
            }
            
            extraDataObjectsCreated.Add(newObj);
        }
    }
    
    private ExtraDataDocument GetExtraDataDocument(string guid)
    {
        AllExtraDataDocument allExtraDataDocument = new AllExtraDataDocument();
        
        JsonUtility.FromJsonOverwrite(extraDataDocuments.text, allExtraDataDocument);
        
        for (int i = 0; i < allExtraDataDocument.allExtraData.Count; i++)
        {
            if (allExtraDataDocument.allExtraData[i].guidDocument.Equals(guid))
                return allExtraDataDocument.allExtraData[i];
        }

        return null;
    }

    private GameObject FindObjectInChildren(GameObject parent, string nameObj)
    {
        GameObject obj = null;

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name.Equals(nameObj))
            {
                obj = parent.transform.GetChild(i).gameObject;
                break;
            }
            
            obj = FindObjectInChildren(parent.transform.GetChild(i).gameObject, nameObj);
            if (obj != null) break;
        }

        return obj;
    }

    private void SaveExtraDataPhoto()
    {
        Photo photoNode = documentNode as Photo;

        for (int i = 0; i < extraDataObjectsCreated.Count; i++)
        {
            photoNode.extraDataDocument.extraData[i].rtData =
                new RectTransformData(extraDataObjectsCreated[i].GetComponent<RectTransform>());
            photoNode.extraDataDocument.extraData[i].siblingIndex = extraDataObjectsCreated[i].transform.GetSiblingIndex();
        }
        
        photoNode.extraDataDocument.guidDocument = photoNode.guid;

        AllExtraDataDocument allExtraDataDocument = new AllExtraDataDocument();
        
        JsonUtility.FromJsonOverwrite(extraDataDocuments.text, allExtraDataDocument);

        bool exist = false;
        
        for (int i = 0; i < allExtraDataDocument.allExtraData.Count; i++)
        {
            if (allExtraDataDocument.allExtraData[i].guidDocument.Equals(photoNode.guid))
            {
                allExtraDataDocument.allExtraData[i] = photoNode.extraDataDocument;
                exist = true;
                break;
            }
        }
        
        if (!exist) allExtraDataDocument.allExtraData.Add(photoNode.extraDataDocument);

        string jsonAllExtraData = JsonUtility.ToJson(allExtraDataDocument);

        System.IO.File.WriteAllText(AssetDatabase.GetAssetPath(extraDataDocuments), jsonAllExtraData);
        
        AssetDatabase.Refresh();

        Debug.Log("Photo Saved");
    }

    private void SetRectTransformData(RectTransform rt, RectTransformData rtData)
    {
        RectTransform rectTransform = rt.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = rtData.anchoredPosition;
        rectTransform.sizeDelta = rtData.sizeDelta;
        rectTransform.pivot = rtData.pivot;
        rectTransform.anchorMin = rtData.anchorMin;
        rectTransform.anchorMax = rtData.anchorMax;
        rectTransform.localEulerAngles = rtData.rotation;
        rectTransform.localScale = rtData.scale;
    }
}
