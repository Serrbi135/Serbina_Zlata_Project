using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    private void Awake()
    {
        group.alpha = 0;
    }

    public void Show()
    {
        group.DOFade(1, 0.5f);
    }

    public void Hide()
    {
        group.DOFade(0, 0.5f);
    }
}
