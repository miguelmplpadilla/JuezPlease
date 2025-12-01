using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Resources.Scripts.NPC
{
    public class TelephoneController : NPCController
    {
        public RectTransform rtImagePhone;
        public RectTransform rtImageCable;

        protected override void Interact(InteractNPCEvent i)
        {
            if (!i.obj.Equals(gameObject)) return;

            StartCoroutine(AnimationCall(i));
        }

        private IEnumerator AnimationCall(InteractNPCEvent i)
        {
            DialogController.instance.isSpeaking = true;
            
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
            
            DialogController.instance.isSpeaking = false;
            
            base.Interact(i);
        }

        public override IEnumerator ShowHideNPCAnimation(bool show, bool isThisSpeaking)
        {
            if (!show) while (DialogController.instance.isSpeaking && isThisSpeaking) yield return null;
            
            rtImagePhone.DOKill();
            rtImageCable.DOKill();
            
            yield return null;
            
            rtImagePhone.DOAnchorPosY(show ? 0 : -rtImagePhone.rect.size.y, 0.4f);
            rtImageCable.DOAnchorPosY(show ? 0 : -rtImagePhone.rect.size.y, 0.6f);
        }
    }
}