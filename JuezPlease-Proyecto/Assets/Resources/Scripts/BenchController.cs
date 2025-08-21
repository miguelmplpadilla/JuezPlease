using System;
using DG.Tweening;
using UnityEngine;

public class BenchController : MonoBehaviour
{
    public static BenchController instance;
    
    public GameObject tableLeft;
    public GameObject tableRight;

    public GameObject wintessTable;

    private bool hasShow = false;

    private Vector2 originalPositionTableLeft;
    private Vector2 originalPositionTableRight;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalPositionTableLeft = tableLeft.transform.localPosition;
        originalPositionTableRight = tableRight.transform.localPosition;
        
        SetInitialScaleAndPosition();
    }

    public void CallWitness()
    {
        tableLeft.transform.DOLocalMoveX(hasShow ? -390 : originalPositionTableLeft.x, 0.6f);
        tableRight.transform.DOLocalMoveX(hasShow ? 390 : originalPositionTableRight.x, 0.6f);
        
        wintessTable.transform.DOScale(hasShow ? 0 : 1, 0.6f);
        
        hasShow = !hasShow;
    }

    private void SetInitialScaleAndPosition()
    {
        tableLeft.transform.localPosition = new Vector2(-390, originalPositionTableLeft.y);
        tableRight.transform.localPosition = new Vector2(390, originalPositionTableRight.y);
        wintessTable.transform.localScale = Vector3.zero;
    }
}
