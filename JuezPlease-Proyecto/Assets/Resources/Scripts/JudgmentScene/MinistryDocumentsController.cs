using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Resources.Scripts.JudgmentScene
{
    public class MinistryDocumentsController : MonoBehaviour
    {
        public static MinistryDocumentsController instance;

        public Document documentToCreate;
        
        public List<MinistryStamps> ministryStamps;

        [SerializeField] private GameObject prefabStampMinistry;
        
        public enum Ministry
        {
            NONE, JUSTICE, INTERIOR, DEFENSE, WORK, HEALTH, EDUCATION, INFORMATION, ECONOMY
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            StartCoroutine(DocumentCreator.instance.CreateDocument(documentToCreate, 0));
        }

        public MinistryStamps GetMinistryStamps(Ministry ministry)
        {
            return ministryStamps.Find(it => it.ministry == ministry);
        }

        public void CreateStamp(Ministry ministry, RectTransform parent, bool isCorrect)
        {
            MinistryStamps stamps = GetMinistryStamps(ministry);

            GameObject stamp = Instantiate(prefabStampMinistry, parent.transform);

            stamp.GetComponent<Image>().sprite = isCorrect
                ? stamps.correctStamp
                : stamps.falseStamps[Random.Range(0, stamps.falseStamps.Count)];

            var stampRt = stamp.GetComponent<RectTransform>();
            stampRt.sizeDelta = parent.rect.size;

            stamp.transform.DORotate(new Vector3(0, 0, Random.Range(-15, 15)), 0);
            stamp.transform.localPosition = Vector3.zero;
        }
    }
    
    [Serializable]
    public class MinistryStamps
    {
        public MinistryDocumentsController.Ministry ministry;
        public Sprite correctStamp;
        public List<Sprite> falseStamps;
    }
}