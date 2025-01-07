using System;
using DG.Tweening;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public GameObject tableLeft;
    public GameObject tableRight;

    public GameObject wintessTable;

    public RectTransform drawer;

    private bool hasShow = false;

    private Vector2 originalPositionTableLeft;
    private Vector2 originalPositionTableRight;
    
    private void Start()
    {
        originalPositionTableLeft = tableLeft.transform.localPosition;
        originalPositionTableRight = tableRight.transform.localPosition;
    }
    
    private void Update()
    {
        Debug.Log(drawer.anchoredPosition);
        if (Input.GetKeyDown(KeyCode.S))
            ShowDrawer();
    }

    private void ShowDrawer()
    {
        drawer.DOAnchorPosY(!hasShow ? 0 : -570, 0.6f).SetEase(Ease.InOutBack);
        hasShow = !hasShow;
    }

    private void ChangeScalesTables()
    {
        tableLeft.transform.DOLocalMoveX(hasShow ? -390 : originalPositionTableLeft.x, 0.6f);
        tableRight.transform.DOLocalMoveX(hasShow ? 390 : originalPositionTableRight.x, 0.6f);
        
        wintessTable.transform.DOScale(hasShow ? 0 : 1, 1);
        
        hasShow = !hasShow;
    }
}
