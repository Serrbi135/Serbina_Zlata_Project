using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public Button restartButton;
    public Button exitButton;
    void Start()
    {
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(ExitScene);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ExitScene()
    {
        SceneManager.LoadScene("LoadScene");
    }

    
}
