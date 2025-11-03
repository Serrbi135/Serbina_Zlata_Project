using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class preFinal : MonoBehaviour
{

    public Button nextButton;
    public CanvasGroup cv;


    void Start()
    {
        nextButton.onClick.AddListener(Next);
        cv.DOFade(1, 2f);
    }


    private void Next()
    {
        Loader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
