using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.NPC
{
    public class TelephoneController : NPCController
    {
        public static TelephoneController instance;
        public RectTransform rtImagePhone;
        public RectTransform rtImageCable;

        public RectTransform rtHangUpButton;
        public Button hangUpButton;

        public bool canHide = true;
        public bool isCalled = false;
        
        public int phoneNumberCalled = -1;

        private void Awake()
        {
            EventBus<HideButtonsCloseEvent>.Register(new EventBinding<HideButtonsCloseEvent>(HideButtonHidePhone, gameObject));
        }

        public IEnumerator AnimationCall()
        {
            Sequence sequencePhone = DOTween.Sequence();
            sequencePhone.Append(rtImagePhone.DOAnchorPosY(-10, 0.45f));
            sequencePhone.Append(rtImagePhone.DOAnchorPosY(0, 0.4f).SetDelay(0.1f));
            sequencePhone.Append(rtImagePhone.DOAnchorPosY(-10, 0.4f).SetDelay(0.1f));
            sequencePhone.Append(rtImagePhone.DOAnchorPosY(0, 0.4f));

            yield return new WaitForSeconds(0.1f);
            
            Sequence sequenceCable = DOTween.Sequence();
            sequenceCable.Append(rtImageCable.DOAnchorPosY(-10, 0.4f));
            sequenceCable.Append(rtImageCable.DOAnchorPosY(0, 0.4f).SetDelay(0.1f));
            sequenceCable.Append(rtImageCable.DOAnchorPosY(-10, 0.4f).SetDelay(0.1f));
            sequenceCable.Append(rtImageCable.DOAnchorPosY(0, 0.4f));
            
            yield return new WaitForSeconds(2.1f);
        }

        public void StartCall(int number)
        {
            StartCoroutine(Call(number));
        }

        private IEnumerator Call(int number)
        {
            phoneNumberCalled = number;
            canHide = false;
            
            DialogController.instance.isSpeaking = true;

            yield return AnimationCall();
            
            DialogController.instance.isSpeaking = false;
            isCalled = true;
        }

        public override IEnumerator ShowHideNPCAnimation(bool show)
        {
            if (!canHide) yield break;
            
            rtImagePhone.DOKill();
            rtImageCable.DOKill();
            
            yield return null;
            
            rtImagePhone.DOAnchorPosY(0, 0.4f);
            rtImageCable.DOAnchorPosY(0, 0.6f);
        }

        public void HidePhone()
        {
            phoneNumberCalled = -1;
            
            rtImagePhone.DOAnchorPosY(-rtImagePhone.rect.size.y, 0.4f);
            rtImageCable.DOAnchorPosY(-rtImagePhone.rect.size.y, 0.6f);

            canHide = true;
            isCalled = false;
        }
        
        private void HideButtonHidePhone(HideButtonsCloseEvent e)
        {
            hangUpButton.interactable = !e.hide;
            rtHangUpButton.DOAnchorPosX(
                e.hide ? 0 : -70, 0.6f);
        }
        
        public void HideButtonHidePhone(bool hide)
        {
            HideButtonHidePhone(new HideButtonsCloseEvent { hide = hide });
        }
    }
}