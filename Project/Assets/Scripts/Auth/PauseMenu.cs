using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    /*[Header("������ ����")]
    public GameObject pauseMenuPanel;

    [Header("������ ��������� ����")]
    public Button saveButton;
    public Button exitButton;

    private SaveLoadManager saveLoadManager;
    private bool isPaused = false;

    private void Start()
    {
        // ������� �������� ����������
        saveLoadManager = FindObjectOfType<SaveLoadManager>();

        // ��������� ������ ��������� ����
        saveButton.onClick.AddListener(OpenSaveMenu);
        exitButton.onClick.AddListener(ExitToMainMenu);

        // ���������� �������� ��� ����
        pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        // ��������/�������� ���� ����� �� ������� ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        //pauseMenuPanel.SetActive(true);
        //Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        //pauseMenuPanel.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OpenSaveMenu()
    {
        pauseMenuPanel.SetActive(false);
    }


    private void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LoadScene");
    }*/

}
