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
            case 4 or 5 or 6 or 7 or 8: return "Понедельник";
            case 10: return "Сон понедельника";
            case 12 or 13 or 14: return "Вторник";
            case 16: return "Ночь вторника";
            case 18 or 19 or 20: return "Среда";
            case 22: return "Ночь среды";
            case 24: return "Четверг";
            case 26: return "Ночь четверга";
            case 28 or 29: return "Пятница";
            case 31: return "Ночь пятницы";
            case 33: return "Суббота";
            case 35: return "Ночь субботы";
            case 37 or 28: return "Финал";
            case -1: return "Сохранения нет";
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
