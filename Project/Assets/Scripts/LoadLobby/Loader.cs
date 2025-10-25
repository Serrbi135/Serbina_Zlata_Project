using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static string nextSceneName;
    public static int nextSceneIndex = -1;

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        nextSceneIndex = -1;
        SceneManager.LoadScene("LoadLobby");
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        nextSceneName = null;
        SceneManager.LoadScene("LoadLobby");
    }
} 
   

