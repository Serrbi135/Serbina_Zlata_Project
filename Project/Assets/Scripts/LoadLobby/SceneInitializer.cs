using UnityEngine;
using System.Linq;

public class SceneInitializer : MonoBehaviour
{
    private void Start()
    {
        InitializeLoadedProgress();

        CleanupLoadData();
    }

    private void InitializeLoadedProgress()
    {
        Debug.Log("SceneInitializer: начал инициализацию прогресса");

        if (Loader.loadedProgress != null)
        {
            Debug.Log("SceneInitializer: нашел прогресс в Loader.loadedProgress");
            ApplyProgress(Loader.loadedProgress);
        }
        else if (PlayerPrefs.HasKey("LoadedSceneIndex"))
        {
            Debug.Log("SceneInitializer: нашел прогресс в PlayerPrefs");

            int[] diaryFlags;
            string diaryFlagsStr = PlayerPrefs.GetString("LoadedDiaryFlags", "");
            if (!string.IsNullOrEmpty(diaryFlagsStr))
            {
                diaryFlags = diaryFlagsStr.Split(',').Select(s =>
                    string.IsNullOrEmpty(s) ? 0 : int.Parse(s)).ToArray();
            }
            else
            {
                diaryFlags = new int[20];
            }

            var progress = new PlayerProgress
            {
                sceneIndex = PlayerPrefs.GetInt("LoadedSceneIndex"),
                moralityPoints = PlayerPrefs.GetInt("LoadedMorality"),
                diaryFlags = diaryFlags,
                playTime = PlayerPrefs.GetFloat("LoadedPlayTime")
            };

            ApplyProgress(progress);
        }
        else
        {
            Debug.Log("SceneInitializer: прогресс для загрузки не найден, начинаем с начальных значений");
        }
    }

    private void ApplyProgress(PlayerProgress progress)
    {
        Debug.Log($"SceneInitializer: применяю прогресс - сцена {progress.sceneIndex}, мораль {progress.moralityPoints}");

        if (MoralitySystem.Instance != null)
        {
            MoralitySystem.Instance.SetPoints(progress.moralityPoints);
            Debug.Log($"MoralitySystem инициализирован: {progress.moralityPoints} очков");
        }

        if (DiaryManager.Instance != null && progress.diaryFlags != null)
        {
            DiaryManager.Instance.SetFlags(progress.diaryFlags);
            Debug.Log($"DiaryManager инициализирован: {progress.diaryFlags.Length} флагов");

            for (int i = 0; i < progress.diaryFlags.Length; i++)
            {
                if (progress.diaryFlags[i] == 1)
                {
                    DiaryManager.Instance.UnlockEntry(i);
                }
            }
        }

        var saveManager = FindObjectOfType<SaveLoadManager>();
        if (saveManager != null)
        {
            saveManager.LoadSavedPlayTime(progress.playTime);
        }

        Debug.Log("SceneInitializer: прогресс полностью применен на сцене");
    }

    private void CleanupLoadData()
    {
        if (Loader.isLoadingGame || Loader.loadedProgress != null)
        {
            Debug.Log("SceneInitializer: очищаю данные загрузки");
            Loader.ClearLoadData();

            PlayerPrefs.DeleteKey("LoadedSceneIndex");
            PlayerPrefs.DeleteKey("LoadedMorality");
            PlayerPrefs.DeleteKey("LoadedDiaryFlags");
            PlayerPrefs.DeleteKey("LoadedPlayTime");
            PlayerPrefs.Save();
        }
    }
}