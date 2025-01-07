using DG.Tweening;
using UnityEngine;

public class BenchController : MonoBehaviour
{
    public GameObject tableLeft;
    public GameObject tableRight;

    public GameObject wintessTable;

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
        if (Input.GetKeyDown(KeyCode.S))
            CallWitness();
    }

    private void CallWitness()
    {
        tableLeft.transform.DOLocalMoveX(hasShow ? -390 : originalPositionTableLeft.x, 0.6f);
        tableRight.transform.DOLocalMoveX(hasShow ? 390 : originalPositionTableRight.x, 0.6f);
        
        wintessTable.transform.DOScale(hasShow ? 0 : 1, 0.6f);
        
        hasShow = !hasShow;
    }
}
