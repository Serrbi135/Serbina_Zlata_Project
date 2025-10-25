using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatsDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text sceneInfoText;
    public TMP_Text diaryFlagsText;
    public TMP_Text playTimeText;

    private GameAPIManager apiManager;
    private int currentUserId;

    void Start()
    {
        currentUserId = PlayerPrefs.GetInt("CurrentUserId");
        apiManager = FindObjectOfType<GameAPIManager>();

        LoadAndDisplaySaveStats();
    }

    void LoadAndDisplaySaveStats()
    {
        if (apiManager != null)
        {
            StartCoroutine(apiManager.LoadGame(currentUserId, (progress) =>
            {
                if (progress != null)
                {
                    UpdateUI(progress);
                }
                else
                {
                    ShowNoDataMessage();
                }
            }));
        }
        else
        {
            Debug.LogError("GameAPIManager not found!");
            ShowNoDataMessage();
        }
    }

    void UpdateUI(PlayerProgress progress)
    {
        string sceneName = GetSceneNameByIndex(progress.sceneIndex);
        sceneInfoText.text = $"Текущая сцена: {sceneName}";
        int unlockedFlagsCount = CountUnlockedDiaryFlags(progress.diaryFlags);
        diaryFlagsText.text = $"Открыто записей в дневнике: {unlockedFlagsCount}";
        string timeFormatted = FormatTime(progress.playTime);
        playTimeText.text = $"Общее время игры: {timeFormatted}";
    }

    void ShowNoDataMessage()
    {
        sceneInfoText.text = "Данные сохранения не найдены";
        diaryFlagsText.text = "";
        playTimeText.text = "";
    }

    private string GetSceneNameByIndex(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0: return "Регистрация";
            case 1: return "Меню загрузки";
            case 2: return "короч лее";
            case 3: return "Лпон";
            default: return $"Сцена {sceneIndex}";
        }
    }

    private int CountUnlockedDiaryFlags(int[] diaryFlags)
    {
        if (diaryFlags == null) return 0;

        int count = 0;
        foreach (int flag in diaryFlags)
        {
            if (flag == 1) count++;
        }
        return count;
    }

    private string FormatTime(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);

        return $"{hours:00}:{minutes:00}";
    }


    public void ExitButton()
    {
        Loader.LoadScene("LoadScene");
    }

}
