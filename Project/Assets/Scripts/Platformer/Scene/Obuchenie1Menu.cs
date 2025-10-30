using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class Obuchenie1Menu : MonoBehaviour
{
    public Button restartButton;
    public Button exitButton;
    void Start()
    {
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(ExitScene);
    }

    public void Restart()
    {
        Loader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitScene()
    {
        Loader.LoadScene("LoadScene");
    }

}
