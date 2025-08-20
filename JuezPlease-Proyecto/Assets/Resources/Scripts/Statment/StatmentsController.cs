using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatmentsController : MonoBehaviour
{
    public GameObject statmentPrefab;
    
    public bool isAnimating = false;
    public bool isShowed = false;
    
    public RectTransform continer;
    public GameObject continerStatments;

    public Button dropDownButton;

    private void Start()
    {
        List<Statment> startStatments =
            new List<Statment>(JudgedSceneController.instance.allStartStatments.allstatmentsStart);
        
        for (int i = 0; i < startStatments.Count; i++)
        {
            Statment statment = startStatments[i];
            GameObject statmentObject = Instantiate(statmentPrefab, continerStatments.transform);
            
            DragStatmentController dragStatmentController = statmentObject.GetComponent<DragStatmentController>();
            dragStatmentController.document = statment;
            
            dragStatmentController.textStatment.text = statment.textStatment.value;
            dragStatmentController.textNumber.text = (i + 1).ToString();
        }
        
        dropDownButton.onClick.AddListener(() =>
        { ShowStatments(); });
    }

    public void ShowStatments()
    {
        if (isAnimating) return;
        
        float yFinalPosition = 0;
        if (isShowed) yFinalPosition = 433;

        isAnimating = true;

        continer.DOAnchorPosY(yFinalPosition, 0.5f).OnComplete(() =>
        {
            dropDownButton.transform.localScale = new Vector3(1, -dropDownButton.transform.localScale.y, 1);
            isAnimating = false;
        }).SetEase(Ease.InOutBack);
        
        isShowed = !isShowed;
    }
}
