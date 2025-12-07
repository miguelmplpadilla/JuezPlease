using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BenchController : MonoBehaviour
{
    public static BenchController instance;
    
    public GameObject tableLeft;
    public GameObject tableRight;

    public GameObject wintessTable;

    public bool witnessShowed = false;

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

    public IEnumerator CallWitness(CharacterData characterData)
    {
        if (characterData != null)
        {
            //TODO: Setear sprites en el testigo
        }
        
        tableLeft.transform.DOLocalMoveX(witnessShowed ? -390 : originalPositionTableLeft.x, 0.6f);
        tableRight.transform.DOLocalMoveX(witnessShowed ? 390 : originalPositionTableRight.x, 0.6f);
        
        wintessTable.transform.DOScale(witnessShowed ? 0 : 1, 0.6f);

        yield return new WaitForSeconds(0.6f);
        
        witnessShowed = !witnessShowed;

        if (!witnessShowed) StartCoroutine(CallWitness(null));
    }

    private void SetInitialScaleAndPosition()
    {
        tableLeft.transform.localPosition = new Vector2(-390, originalPositionTableLeft.y);
        tableRight.transform.localPosition = new Vector2(390, originalPositionTableRight.y);
        wintessTable.transform.localScale = Vector3.zero;
    }
}
