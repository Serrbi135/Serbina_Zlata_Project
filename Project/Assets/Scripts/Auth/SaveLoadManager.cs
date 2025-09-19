using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class SaveLoadManager : MonoBehaviour
{
    [Header("UI Elements")]

    private GameObject currentPanel;
    public GameObject saveLoadPanelPrefab;
    public Button saveButton;
    public Button exitButton;

    private GameAPIManager apiManager;
    public  SaveLoadManager Instance;
    private int currentUserId;

    private void Start()
    {
        apiManager = FindObjectOfType<GameAPIManager>();
        currentUserId = PlayerPrefs.GetInt("CurrentUserId");
        currentPanel = Instantiate(saveLoadPanelPrefab);

        currentPanel.transform.SetParent(transform, false);
        Canvas canvas = currentPanel.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main; 
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = 100;
        }
        else
        {
            Debug.LogError("Canvas component missing!");
        }

        saveButton = currentPanel.transform.Find("saveButton").GetComponent<Button>();
        exitButton = currentPanel.transform.Find("exitButton").GetComponent<Button>();

        if (saveButton == null || exitButton == null)
        {
            Debug.LogError("Не найдены кнопки в префабе меню!");
            return;
        }

        saveButton.onClick.AddListener(SaveGame);
        exitButton.onClick.AddListener(ExitFrom);

        currentPanel.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !(SceneManager.GetActiveScene().name == "LoadScene") && !(SceneManager.GetActiveScene().name == "Registration") && !(SceneManager.GetActiveScene().name == "Diary"))
        {
            ToggleMenu();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void ToggleMenu()
    {
        bool isActive = !currentPanel.activeSelf;
        currentPanel.SetActive(isActive);

        Time.timeScale = isActive ? 0f : 1f;
    }



    private void ExitFrom()
    {
        currentPanel.SetActive(false);
        SceneManager.LoadScene("LoadScene");
    }

    public void SaveGame()
    {
        PlayerProgress progress = GetCurrentProgress();

        StartCoroutine(apiManager.SaveGame(
            currentUserId,
            progress,
            (success) => {
                if (success)
                {
                    Debug.Log("Game saved successfully!");
                }
                else
                {
                    
                    Debug.LogError($"Save failed");
                }
            }
        ));
    }

    private PlayerProgress GetCurrentProgress()
    {
        return new PlayerProgress
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
            moralityPoints = MoralitySystem.Instance.Points,
            diaryFlags = DiaryManager.Instance.GetFlags()
        };
    }
}