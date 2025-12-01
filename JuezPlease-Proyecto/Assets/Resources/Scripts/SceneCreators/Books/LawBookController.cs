using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.JudgmentCreator.ScriptableObjects;
using Resources.Scripts.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LawBookController : MonoBehaviour
{
    public LawBookNode lawBookNode;
    
    public List<GameObject> pagesIntantiated;

    public GameObject pagePrefab;
    public GameObject blanckPagePrefab;
    public GameObject lawPrefab;

    public GameObject continerPages;
    
    public int index = 0;

    public GameObject leftPosition;
    public GameObject rightPosition;

    public Button turnPageLeftButton;
    public Button turnPageRightButton;

    public GameObject indicatorTurnPageLeft;
    public GameObject indicatorTurnPageRight;
    
    public TextMeshProUGUI pageNumberText;

    public Button[] tagButtons;
    public Button[] indexButtons;

    private void Start()
    {
        FindLawBookNode();
        
        turnPageLeftButton.onClick.AddListener(() => SetIndex(-2));
        turnPageRightButton.onClick.AddListener(() => SetIndex(2));
        
        CreatePages();
        
        SetIndex(0);

        foreach (var tagButton in tagButtons)
        {
            tagButton.onClick.AddListener(() => MoveToTag(tagButton.name.Replace("Tag", "").ToLower()));
            tagButton.gameObject.SetActive(false);
        }

        foreach (var indexButton in indexButtons)
            indexButton.onClick.AddListener(() =>
                MoveToTag(indexButton.transform.parent.name.Replace("Laws", "").ToLower()));

        foreach (var tagButton in tagButtons)
        {
            string buttonTag = tagButton.name.Replace("Tag", "").ToLower();
            foreach (var page in pagesIntantiated)
            {
                if (page.name.Contains(buttonTag))
                {
                    tagButton.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void Update()
    {
        pageNumberText.text = (index+1) + " / " + pagesIntantiated.Count;
    }

    private void CreatePages()
    {
        CreatePageLaws(lawBookNode.estLaws, "est", GeneralTraductions.estTitlePage);
        CreatePageLaws(lawBookNode.civLaws, "civ", GeneralTraductions.civTitlePage);
        CreatePageLaws(lawBookNode.socLaws, "soc", GeneralTraductions.socTitlePage);
        CreatePageLaws(lawBookNode.ecoLaws, "eco", GeneralTraductions.ecoTitlePage);
        CreatePageLaws(lawBookNode.grvLaws, "grv", GeneralTraductions.grvTitlePage);
    }
    
    private void CreatePageLaws(List<LawData> laws, string suffix, LocalizableString titlePage)
    {
        int count = 0;
        int cantPage = 0;
        GameObject page = null;
        for (int i = 0; i < laws.Count; i++)
        {
            if (count == 0)
            {
                if (page != null)
                {
                    var layoutGroup = page.transform.GetChild(1).GetComponent<VerticalLayoutGroup>();
                    if (layoutGroup != null) StartCoroutine(RestartVerticalLayout(layoutGroup));
                }
                
                cantPage++;
                page = Instantiate(pagePrefab, continerPages.transform);
                page.name = suffix + "" + cantPage;
                page.transform.GetChild(0).GetComponent<LocalizableController>().SetText(titlePage);
                
                pagesIntantiated.Add(page);
            }

            if (page == null)
            {
                cantPage = 0;
                continue;
            }

            CreateLaw(laws[i], page.transform.GetChild(1));
            
            count++;

            if (count == 2) count = 0;
        }
        
        if (pagesIntantiated.Count % 2 != 0)
            pagesIntantiated.Add(Instantiate(blanckPagePrefab, continerPages.transform));
    }

    private GameObject CreateLaw(LawData lawData, Transform parent)
    {
        GameObject lawObj = Instantiate(lawPrefab, parent);
        lawObj.transform.GetChild(0).GetComponent<LocalizableController>().SetText(lawData.titleLaw);
        lawObj.transform.GetChild(1).GetComponent<LocalizableController>().SetText(lawData.descriptionLaw);
        lawObj.transform.GetChild(2).GetComponent<Image>().color = lawData.colorPanelSentenceAmount;
        lawObj.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = lawData.sentenceAmount.x + "-" + lawData.sentenceAmount.y;
        lawObj.transform.GetChild(2).GetChild(1).GetComponent<LocalizableController>().SetText(GetTitleAmountLaw(lawData.sentence));

        return lawObj;
    }

    private LocalizableString GetTitleAmountLaw(StartJudgmentNode.Sentence sentence)
    {
        switch (sentence)
        {
            case StartJudgmentNode.Sentence.MONEY:
                return GeneralTraductions.monetaryFineLaw;
            case StartJudgmentNode.Sentence.PRISON:
                return GeneralTraductions.prisonYearsLaw;
            case StartJudgmentNode.Sentence.WORK:
                return GeneralTraductions.workDaisLaw;
            case StartJudgmentNode.Sentence.DEATH:
                return GeneralTraductions.deathLaw;
            
            case StartJudgmentNode.Sentence.PSYCHIATRIC:
                return GeneralTraductions.psychiatricLaw;
            case StartJudgmentNode.Sentence.LOBOTOMY:
                return GeneralTraductions.lobotomyLaw;
            case StartJudgmentNode.Sentence.ELECTROSHOCK:
                return GeneralTraductions.electroshockLaw;
            case StartJudgmentNode.Sentence.CHEMICALSTRAITJACKET:
                return GeneralTraductions.chemicalstraitJacketLaw;
            
            default:
                return GeneralTraductions.prisonYearsLaw;
        }
    }

    private IEnumerator RestartVerticalLayout(VerticalLayoutGroup verticalLayout)
    {
        verticalLayout.enabled = false;
        yield return new WaitForEndOfFrame();
        verticalLayout.enabled = true;
    }

    private void SetPage()
    {
        foreach (var page in pagesIntantiated)
            page.transform.localScale = Vector3.zero;
        
        foreach (var tagButton in tagButtons)
            tagButton.transform.GetChild(0).gameObject.SetActive(false);

        if (index - 1 >= 0)
        {
            StartCoroutine(ActivePage(index-1, leftPosition));

            foreach (var tagButton in tagButtons)
            {
                if (pagesIntantiated[index-1].name.Contains(tagButton.name.Replace("Tag", "").ToLower()))
                    tagButton.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        
        StartCoroutine(ActivePage(index, rightPosition));
    }

    private IEnumerator ActivePage(int i, GameObject positionPage)
    {
        GameObject page = pagesIntantiated[i];
        page.transform.position = positionPage.transform.position;

        if (page.transform.childCount > 0)
        {
            var layoutGroup = page.transform.GetChild(1).GetComponent<VerticalLayoutGroup>();
            if (layoutGroup != null) yield return RestartVerticalLayout(layoutGroup);
        }

        yield return new WaitForEndOfFrame();
        
        page.transform.localScale = Vector3.one;
    }
    
    public void SetIndex(int value)
    {
        index += value;
        
        indicatorTurnPageLeft.SetActive(true);
        indicatorTurnPageRight.SetActive(true);

        if (index <= 1)
        {
            index = 1;
            indicatorTurnPageLeft.SetActive(false);
        } else if (index+1 >= pagesIntantiated.Count)
        {
            index = pagesIntantiated.Count - 1;
            indicatorTurnPageRight.SetActive(false);
        }
        
        if (pagesIntantiated.Count == 2)
        {
            indicatorTurnPageRight.SetActive(false);
            indicatorTurnPageLeft.SetActive(false);
        }
         
        SetPage();
    }

    public void MoveToTag(string tagDelict)
    {
        for (int i = 0; i < pagesIntantiated.Count; i++)
        {
            if (pagesIntantiated[i].name.Contains(tagDelict))
            {
                index = i+1;
                SetIndex(0);
                return;
            }
        }
    }
    
    private void FindLawBookNode()
    {
        if (JudgedSceneController.instance == null) return;
        lawBookNode = JudgedSceneController.instance.judgmentCreator.nodes.Find(it => it is LawBookNode) as LawBookNode;
    }
}
