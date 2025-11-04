using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static string nextSceneName
    {
        get => PlayerPrefs.GetString("NextSceneName", "");
        set => PlayerPrefs.SetString("NextSceneName", value);
    }

    public static int nextSceneIndex
    {
        get => PlayerPrefs.GetInt("NextSceneIndex", -1);
        set => PlayerPrefs.SetInt("NextSceneIndex", value);
    }

    public static bool isLoadingGame
    {
        get => PlayerPrefs.GetInt("IsLoadingGame", 0) == 1;
        set => PlayerPrefs.SetInt("IsLoadingGame", value ? 1 : 0);
    }

    public static PlayerProgress loadedProgress
    {
        get
        {
            string json = PlayerPrefs.GetString("LoadedProgress", "");
            if (!string.IsNullOrEmpty(json))
            {
                return JsonUtility.FromJson<PlayerProgress>(json);
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                string json = JsonUtility.ToJson(value);
                PlayerPrefs.SetString("LoadedProgress", json);
            }
            else
            {
                PlayerPrefs.DeleteKey("LoadedProgress");
            }
            PlayerPrefs.Save();
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        nextSceneIndex = -1;
        isLoadingGame = false;
        loadedProgress = null;
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadLobby");
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        nextSceneName = "";
        isLoadingGame = false;
        loadedProgress = null;
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadLobby");
    }

    public static void LoadGame(PlayerProgress progress)
    {
        loadedProgress = progress;
        isLoadingGame = true;

        if (progress != null && progress.sceneIndex >= 0)
        {
            nextSceneIndex = progress.sceneIndex;
            nextSceneName = "";
        }
        else
        {
            nextSceneIndex = 4;
            nextSceneName = "";
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadLobby");
    }

    public static void ClearLoadData()
    {
        PlayerPrefs.DeleteKey("NextSceneName");
        PlayerPrefs.DeleteKey("NextSceneIndex");
        PlayerPrefs.DeleteKey("IsLoadingGame");
        PlayerPrefs.DeleteKey("LoadedProgress");
        PlayerPrefs.Save();
    }
}