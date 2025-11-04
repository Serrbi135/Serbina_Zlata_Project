using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LoadLobby : MonoBehaviour
{
    [SerializeField] private Image progressCircle;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return null;

        if (Loader.isLoadingGame && Loader.loadedProgress != null)
        {
            ApplyLoadedProgress(Loader.loadedProgress);
        }

        AsyncOperation asyncLoad = null;
        string sceneToLoad = "";
        int sceneIndexToLoad = -1;

        if (!string.IsNullOrEmpty(Loader.nextSceneName))
        {
            sceneToLoad = Loader.nextSceneName;
            Debug.Log($"LoadLobby: загружаю сцену по имени: {sceneToLoad}");
        }
        else if (Loader.nextSceneIndex != -1)
        {
            sceneIndexToLoad = Loader.nextSceneIndex;
            Debug.Log($"LoadLobby: загружаю сцену по индексу: {sceneIndexToLoad}");
        }
        else
        {
            Debug.LogError("LoadLobby: нет сцены для загрузки! Загружаю сцену 0");
            sceneIndexToLoad = 0;
        }

        try
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            }
            else
            {
                asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"LoadLobby: ошибка загрузки сцены: {e.Message}");
            asyncLoad = SceneManager.LoadSceneAsync(0);
        }

        if (asyncLoad == null)
        {
            Debug.LogError("LoadLobby: AsyncOperation is null! Загружаю сцену 0");
            SceneManager.LoadScene(0);
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        float progress = 0;
        float timer = 0f;
        float maxWaitTime = 10f;

        while (!asyncLoad.isDone && timer < maxWaitTime)
        {
            timer += Time.deltaTime;
            progress = Mathf.MoveTowards(progress, asyncLoad.progress, Time.deltaTime);

            if (progressCircle != null)
                progressCircle.fillAmount = progress;

            if (asyncLoad.progress >= 0.9f)
            {
                if (progressCircle != null)
                    progressCircle.fillAmount = 1;

                yield return new WaitForSeconds(0.5f);

                if (Loader.isLoadingGame)
                {
                    Debug.Log("LoadLobby: сохраняем loadedProgress для следующей сцены");
                }

                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        if (timer >= maxWaitTime)
        {
            Debug.LogError("LoadLobby: таймаут загрузки! Принудительная активация.");
            asyncLoad.allowSceneActivation = true;
        }
    }

    private void ApplyLoadedProgress(PlayerProgress progress)
    {
        if (progress != null)
        {
            Debug.Log($"LoadLobby: применяю прогресс - сцена {progress.sceneIndex}, мораль {progress.moralityPoints}");

            if (GameProgress.Instance != null)
            {
                GameProgress.Instance.ApplyLoadedProgress(progress);
            }

            PlayerPrefs.SetInt("LoadedSceneIndex", progress.sceneIndex);
            PlayerPrefs.SetInt("LoadedMorality", progress.moralityPoints);

            string diaryFlagsStr = progress.diaryFlags != null ? string.Join(",", progress.diaryFlags) : "";
            PlayerPrefs.SetString("LoadedDiaryFlags", diaryFlagsStr);

            PlayerPrefs.SetFloat("LoadedPlayTime", progress.playTime);
            PlayerPrefs.Save();

            Debug.Log($"LoadLobby: прогресс сохранен для следующей сцены");
        }
    }

    private void OnDestroy()
    {
        if (Loader.isLoadingGame)
        {
            Loader.nextSceneName = "";
            Loader.nextSceneIndex = -1;
            Loader.isLoadingGame = false;
        }
    }
}