using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadLobby : MonoBehaviour
{
    [SerializeField] private Image progressCircle;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad;

        if (!string.IsNullOrEmpty(Loader.nextSceneName))
        {
            asyncLoad = SceneManager.LoadSceneAsync(Loader.nextSceneName);
        }
        else if (Loader.nextSceneIndex != -1)
        {
            asyncLoad = SceneManager.LoadSceneAsync(Loader.nextSceneIndex);
        }
        else
        {
            Debug.LogError("No scene to load!");
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        float progress = 0;
        while (!asyncLoad.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncLoad.progress, Time.deltaTime);
            progressCircle.fillAmount = progress;

            if (progress >= 0.9f)
            {
                progressCircle.fillAmount = 1;
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        Loader.nextSceneName = null;
        Loader.nextSceneIndex = -1;
    }
}
