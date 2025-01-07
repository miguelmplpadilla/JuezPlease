using System;
using TMPro;
using UnityEngine;

public class PaginateController : MonoBehaviour
{
    public TextMeshProUGUI[] textsPages;

    public string[] texts;

    public int index = 0;

    private void Start()
    {
        SetTexts();
    }

    private void SetTexts()
    {
        for (int i = 0; i < textsPages.Length; i++)
        {
            string text = "";
            if (index + i < texts.Length)
                text = texts[index + i];
            
            textsPages[i].text = text;
        }
    }
    
    public void SetIndex(int value)
    {
        index += value;
        
        if (index < 0) index = 0;
        else if (index >= texts.Length) index = texts.Length - 1;
        
        SetTexts();
    }
}
