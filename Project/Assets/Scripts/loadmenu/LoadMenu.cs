using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Button newGameButton;
    public Button loadGameButton;
    public Button toDiaryButton;
    public Button statOpenButton;
    public Button logoButton;
    public Button toObuchenieButton;
    public TextMeshProUGUI saveInfoText;
    public TextMeshProUGUI logoText;
    [SerializeField] private CanvasGroup popup;

    [Header("Other")]
    private GameAPIManager apiManager;
    private int currentUserId;
    private bool hasSave;

    private int[] flags;

    [SerializeField] private MoralitySystem moralitySystem;
    [SerializeField] private DiaryManager diaryManager;

    private void Start()
    {
        currentUserId = PlayerPrefs.GetInt("CurrentUserId");
        apiManager = FindObjectOfType<GameAPIManager>();
        moralitySystem = FindObjectOfType<MoralitySystem>();
        diaryManager = FindObjectOfType<DiaryManager>();

        newGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGame);
        toDiaryButton.onClick.AddListener(OpenDiary);
        statOpenButton.onClick.AddListener(OpenStats);
        toObuchenieButton.onClick.AddListener(ToObuchenie);

        Time.timeScale = 1f;

        logoButton.onClick.AddListener(Logo);

        popup.DOFade(1, 0.5f);

        CheckForSave();
    }

    public void CheckForSave()
    {
        int userId = PlayerPrefs.GetInt("CurrentUserId");

        StartCoroutine(apiManager.CheckSaveExists(currentUserId, (exists) => {
            hasSave = exists;
            if (exists)
            {
                saveInfoText.text = $"Последнее сохранение найдено!";
                loadGameButton.gameObject.SetActive(true);
            }
            else
            {
                saveInfoText.text = "Сохранений не найдено. Желаете начать новую игру?";
                loadGameButton.gameObject.SetActive(false);
            }
        }));
    }

    private void OpenDiary()
    {
        StartCoroutine(OpenDiaryRoutine());
    }

    private IEnumerator OpenDiaryRoutine()
    {
        yield return StartCoroutine(apiManager.LoadAndApplyDiaryFlags(PlayerPrefs.GetInt("CurrentUserId")));
        Loader.LoadScene("Diary");
    }

    private void OpenStats()
    {
        Loader.LoadScene("Stats");
    }

    public void LoadGame()
    {
        int userId = PlayerPrefs.GetInt("CurrentUserId");
        StartCoroutine(apiManager.LoadGame(userId, (progress) => {
            if (progress != null)
            {
                Debug.Log($"Загружаем игру: сцена {progress.sceneIndex}, мораль {progress.moralityPoints}");

                Loader.LoadGame(progress);
            }
            else
            {
                Debug.LogError("Не удалось загрузить прогресс игры");
            }
        }));
    }

    private void StartNewGame()
    {
        if (hasSave)
        {
            StartCoroutine(DeleteSaveAndStartNewGame());
        }
        else
        {
            StartCoroutine(CreateNewGame());
        }
    }

    private IEnumerator DeleteSaveAndStartNewGame()
    {
        yield return StartCoroutine(apiManager.DeleteSave(currentUserId, () => {
            Debug.Log("Старое сохранение удалено");
        }));

        yield return StartCoroutine(CreateNewGame());
    }

    private IEnumerator CreateNewGame()
    {
        ResetLocalData();

        PlayerProgress initialProgress = new PlayerProgress
        {
            sceneIndex = 4,
            moralityPoints = 50,
            diaryFlags = new int[20],
            playTime = 0f
        };

        bool saveSuccess = false;
        yield return StartCoroutine(apiManager.SaveGame(currentUserId, initialProgress, (success) => {
            saveSuccess = success;
        }));

        if (saveSuccess)
        {
            Debug.Log("Начальное сохранение создано успешно");
            LoadFirstLevel();
        }
        else
        {
            Debug.LogError("Не удалось создать начальное сохранение");
            LoadFirstLevel();
        }
    }

    private void ResetLocalData()
    {
        flags = new int[20];
        for (int i = 0; i < flags.Length; i++)
        {
            flags[i] = 0;
        }

        if (DiaryManager.Instance != null)
        {
            DiaryManager.Instance.SetFlags(flags);
        }

        if (MoralitySystem.Instance != null)
        {
            MoralitySystem.Instance.SetPoints(50);
        }

        SaveLoadManager saveManager = FindObjectOfType<SaveLoadManager>();
        if (saveManager != null)
        {
            saveManager.ResetCurrentSessionTime();
            saveManager.LoadSavedPlayTime(0f);
        }
    }

    private void Logo()
    {
        logoText.gameObject.SetActive(true);
        StartCoroutine(TextDisappear());
    }

    public IEnumerator TextDisappear()
    {
        yield return new WaitForSeconds(7);
        logoText.gameObject.SetActive(false);
    }

    private void LoadFirstLevel()
    {
        Loader.LoadScene(4);
    }

    private void ToObuchenie()
    {
        Loader.LoadScene("Obuchenie");
    }

    public void ToRegButton()
    {
        Loader.LoadScene("Registration");
    }
}