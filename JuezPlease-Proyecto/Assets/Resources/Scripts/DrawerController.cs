using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public RectTransform drawer;

    private bool hasShow = false;
    private bool animatiogDrawer = false;

    public CanvasGroup canvasGroup;

    public void ShowDrawer()
    {
        if (animatiogDrawer) return;

        animatiogDrawer = true;
        canvasGroup.blocksRaycasts = false;

        drawer.transform.DOKill();

        drawer.DOAnchorPosY(!hasShow ? 0 : -570, 0.6f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            animatiogDrawer = false;
            canvasGroup.blocksRaycasts = true;
        });
        
        hasShow = !hasShow;
    }

    public void MakeShowDrawer(bool show)
    {
        if (animatiogDrawer) return;
        
        drawer.DOAnchorPosY(show ? (!hasShow ? -520 : -50)  : (!hasShow ? -570 : 0), 0.3f);
    }
}
