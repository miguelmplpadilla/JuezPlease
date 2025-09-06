using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvelopeController : MonoBehaviour
{
    public GameObject continer;
    private void Start()
    {
        StartCoroutine(EnablePaperSentences());
    }

    private IEnumerator EnablePaperSentences()
    {
        List<SentencePaperController> sentences = new List<SentencePaperController>();
        for (int i = 0; i < continer.transform.childCount; i++)
        {
            continer.transform.GetChild(i).gameObject.SetActive(false);
            sentences.Add(continer.transform.GetChild(i).GetComponent<SentencePaperController>());
        }
        
        float startYPosition = -195f;

        foreach (var sentence in GameManager.instance.sentences)
        {
            SentencePaperController sentenceRt = sentences.Find(it => it.sentence == sentence).GetComponent<SentencePaperController>();
            sentenceRt.gameObject.SetActive(true);
            yield return null;
            sentenceRt.SetPosition(new Vector2(0, startYPosition));
            startYPosition += 130;
        }
    }
}
