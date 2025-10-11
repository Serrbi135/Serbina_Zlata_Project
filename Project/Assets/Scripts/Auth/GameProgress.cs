using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class PlayerProgress
{
    public int sceneIndex;
    public int moralityPoints;
    public int[] diaryFlags;
    public float playTime;
}

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;
    public PlayerProgress CurrentProgress { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveCurrentState()
    {
        CurrentProgress = new PlayerProgress()
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
            moralityPoints = MoralitySystem.Instance.Points,
            diaryFlags = DiaryManager.Instance.GetFlags()
        };
    }
}