using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BenchController : MonoBehaviour
{
    public static BenchController instance;
    
    public GameObject tableLeft;
    public GameObject tableRight;

    public GameObject wintessTable;

    public bool witnessShowed = false;

    private Vector2 originalPositionTableLeft;
    private Vector2 originalPositionTableRight;
    
    private Vector2 originalPositionButtonHideWitness;

    public RectTransform buttonHideWitnessRt;
    public Button buttonHideWitness;

    private void Awake()
    {
        instance = this;
        
        EventBus<HideButtonsCloseEvent>.Register(new EventBinding<HideButtonsCloseEvent>(HideButtonHideWitness, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<HideButtonsCloseEvent>.Deregister(new EventBinding<HideButtonsCloseEvent>(HideButtonHideWitness, gameObject));
    }

    private void Start()
    {
        originalPositionTableLeft = tableLeft.transform.localPosition;
        originalPositionTableRight = tableRight.transform.localPosition;
        
        originalPositionButtonHideWitness = buttonHideWitnessRt.anchoredPosition;
        
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

    public void HideWitness()
    {
        Debug.Log("Hide Witness");
        tableLeft.transform.DOLocalMoveX(-390, 0.6f);
        tableRight.transform.DOLocalMoveX(390, 0.6f);
        
        wintessTable.transform.DOScale(0, 0.6f);

        witnessShowed = false;
    }

    private void SetInitialScaleAndPosition()
    {
        tableLeft.transform.localPosition = new Vector2(-390, originalPositionTableLeft.y);
        tableRight.transform.localPosition = new Vector2(390, originalPositionTableRight.y);
        wintessTable.transform.localScale = Vector3.zero;
    }
    
    private void HideButtonHideWitness(HideButtonsCloseEvent e)
    {
        buttonHideWitness.interactable = !e.hide;
        buttonHideWitnessRt.DOAnchorPosY(
            e.hide ? originalPositionButtonHideWitness.y - 50 : originalPositionButtonHideWitness.y, 0.6f);
    }

    public void HideButtonHideWitness(bool hide)
    {
        HideButtonHideWitness(new HideButtonsCloseEvent { hide = hide });
    }
}
