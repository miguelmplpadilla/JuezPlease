using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatmentsController : MonoBehaviour
{
    public bool isAnimating = false;
    public bool isShowed = false;
    
    public RectTransform continer;
    public GameObject continerStatments;

    public Button dropDownButton;

    private void Start()
    {
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
