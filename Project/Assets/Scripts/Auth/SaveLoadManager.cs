using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;


public class SaveLoadManager : MonoBehaviour
{
    private GameObject currentPanel;
    public GameObject saveLoadPanelPrefab;
    public Button saveButton;
    public Button exitButton;
    public Button toDiaryButton;
    public CanvasGroup savePopup;
    public CanvasGroup diaryUIGame;

    private bool isGameSaved = false;

    private float currentSessionTime = 0f; 
    private bool isCountingTime = false;

    private GameAPIManager apiManager;
    public static SaveLoadManager Instance;
    private int currentUserId;

    private void Start()
    {
        apiManager = FindObjectOfType<GameAPIManager>();
        currentUserId = PlayerPrefs.GetInt("CurrentUserId");
        currentPanel = Instantiate(saveLoadPanelPrefab);

        currentPanel.transform.SetParent(transform, false);
        Canvas canvas = currentPanel.GetComponent<Canvas>();
        
        saveButton = currentPanel.transform.Find("saveButton").GetComponent<Button>();
        exitButton = currentPanel.transform.Find("exitButton").GetComponent<Button>();
        toDiaryButton = currentPanel.transform.Find("toDiaryButton").GetComponent<Button>();
        savePopup = currentPanel.transform.Find("savePopup").GetComponent<CanvasGroup>();
        diaryUIGame = currentPanel.transform.Find("Diary").GetComponent<CanvasGroup>();

        if (saveButton == null || exitButton == null)
        {
            Debug.LogError("Не найдены кнопки в префабе меню!");
            return;
        }

        saveButton.onClick.AddListener(SaveGame);
        exitButton.onClick.AddListener(ExitFrom);
        toDiaryButton.onClick.AddListener(ToDiary);

        currentPanel.SetActive(false);
        StartPlayTime();
        savePopup.alpha = 0;

        diaryUIGame.interactable = false;
        diaryUIGame.blocksRaycasts = false;
        diaryUIGame.alpha = 0;

        UpdateDiaryButtonState();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !(SceneManager.GetActiveScene().name == "LoadScene") && !(SceneManager.GetActiveScene().name == "Obuchenie") && !(SceneManager.GetActiveScene().name == "Obuchenie 1") && !(SceneManager.GetActiveScene().name == "LoadLobby") && !(SceneManager.GetActiveScene().name == "Registration") && !(SceneManager.GetActiveScene().name == "Diary"))
        {
            ToggleMenu();
        }
        if (isCountingTime)
        {
            currentSessionTime += Time.deltaTime;
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

    public void StartPlayTime()
    {
        isCountingTime = true;
    }

    public void PausePlayTime()
    {
        isCountingTime = false;
    }

    public void ResetCurrentSessionTime()
    {
        currentSessionTime = 0f;
    }


    public float GetTotalPlayTime()
    {
        float savedTime = apiManager.GetTotalPlayTime();
        return savedTime + currentSessionTime;
    }

    public void LoadSavedPlayTime(float savedTime)
    {
        apiManager.SetTotalPlayTime(savedTime);
        currentSessionTime = 0f; 
    }
    private void ToggleMenu()
    {
        if (currentPanel.activeSelf)
        {
            isGameSaved = false;
            diaryUIGame.interactable = false;
            diaryUIGame.blocksRaycasts = false;
            diaryUIGame.alpha = 0f;
        }
        bool isActive = !currentPanel.activeSelf;
        
        currentPanel.SetActive(isActive);

        savePopup.alpha = 0;

        if (isActive)
        {
            PausePlayTime();
        }
        else
        {
            StartPlayTime();
        }

        Time.timeScale = isActive ? 0f : 1f;

        UpdateDiaryButtonState();
    }


    private void ExitFrom()
    {
        SaveGame();
        Time.timeScale = 1f;
        currentPanel.SetActive(false);
        StartPlayTime();
        Loader.LoadScene("LoadScene");
    }

    private void ToDiary()
    {
        StartCoroutine(LoadDiaryAndOpen());
    }

    private IEnumerator LoadDiaryAndOpen()
    {
        yield return StartCoroutine(apiManager.LoadAndApplyDiaryFlags(currentUserId));

        diaryUIGame.interactable = true;
        diaryUIGame.blocksRaycasts = true;
        diaryUIGame.alpha = 1f;

        DiaryUIManager diaryUI = diaryUIGame.GetComponent<DiaryUIManager>();
        if (diaryUI != null)
        {
            diaryUI.LoadDiaryEntries();
        }
    }



    public void SaveGame()
    {
        PlayerProgress progress = GetCurrentProgress();
        savePopup.alpha = 1;
        StartCoroutine(apiManager.SaveGame(
            currentUserId,
            progress,
            (success) => {
                if (success)
                {
                    Debug.Log("Game saved successfully!");
                    apiManager.SetTotalPlayTime(GetTotalPlayTime());
                    currentSessionTime = 0f;

                    isGameSaved = true;
                    UpdateDiaryButtonState();
                }
                else
                {
                    
                    Debug.LogError("Save failed");
                }
            }
        ));
    }


    private void UpdateDiaryButtonState()
    {
        if (toDiaryButton != null)
        {
            toDiaryButton.interactable = isGameSaved;

            TextMeshProUGUI buttonText = toDiaryButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = isGameSaved ? "Дневник" : "Сначала сохраните игру";
            }
        }
    }

    public void ResetSaveFlag()
    {
        isGameSaved = false;
        UpdateDiaryButtonState();
    }

    private PlayerProgress GetCurrentProgress()
    {
        return new PlayerProgress
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
            moralityPoints = MoralitySystem.Instance.Points,
            diaryFlags = DiaryManager.Instance.GetFlags(),
            playTime = GetTotalPlayTime()
        };
    }
}