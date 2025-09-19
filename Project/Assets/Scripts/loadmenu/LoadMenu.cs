using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{


    [Header("UI Elements")]
    public Button newGameButton;
    public Button loadGameButton;
    public Button toDiaryButton;

    public Button logoButton;
    public TextMeshProUGUI saveInfoText;

    public TextMeshProUGUI logoText;

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

        logoButton.onClick.AddListener(Logo);

        CheckForSave();
    }

    public void CheckForSave()
    {
        int userId = PlayerPrefs.GetInt("CurrentUserId");

        StartCoroutine(apiManager.CheckSaveExists(currentUserId, (exists) => {
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
        
        StartCoroutine(apiManager.LoadAndApplyDiaryFlags(PlayerPrefs.GetInt("CurrentUserId")));
        SceneManager.LoadScene("Diary");

    }

    public void LoadGame()
    {
        int userId = PlayerPrefs.GetInt("CurrentUserId");
        StartCoroutine(apiManager.LoadGame(userId, (progress) => {
            if (progress != null)
            {
                Debug.Log($"Загружаем игру");
                Debug.Log($"{progress.moralityPoints}");
                if(moralitySystem == null)
                {
                    Debug.Log("uh");
                }
                else
                {
                    moralitySystem.SetPoints(progress.moralityPoints);
                }



                if (diaryManager != null && progress.diaryFlags != null)
                {
                    diaryManager.SetFlags(progress.diaryFlags);
                }

                if (progress.sceneIndex >= 0 && progress.sceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    Debug.Log($"Загрузка сцены с индексом {progress.sceneIndex}");
                    SceneManager.LoadScene(progress.sceneIndex);
                }
            }
        }));
    }

    private void StartNewGame()
    {
        if (hasSave)
        {
            StartCoroutine(apiManager.DeleteSave(currentUserId, () => {
                LoadFirstLevel();
            }));

            for (int i = 0; i < 20; i++)
            {
                flags[i] = 0;
            }
            DiaryManager.Instance.SetFlags(flags);
            MoralitySystem.Instance.SetPoints(0);
            
        }
        else
        {
            LoadFirstLevel();
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
        SceneManager.LoadScene(3); 
        //ПОМЕНЯТЬ
    }

    
}
