using UnityEngine;
using System.Collections;

public class GameExitManager : MonoBehaviour
{
    public static GameExitManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExitGameWithSave()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
        }

        StartCoroutine(DelayedExit());
    }

    public void ExitGameImmediately()
    {
        QuitApplication();
    }

    private IEnumerator DelayedExit()
    {
        yield return new WaitForSecondsRealtime(1f);
        QuitApplication();
    }

    private void QuitApplication()
    {
        Debug.Log("Игра завершена");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
        }
    }
}