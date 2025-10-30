using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeToBlack : MonoBehaviour
{

    [SerializeField] private CanvasGroup group;

    private void Awake()
    {
        group.alpha = 0;
        group.DOFade(1, 4f);
    }
}
