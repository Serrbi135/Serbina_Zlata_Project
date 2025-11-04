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
            CurrentProgress = new PlayerProgress()
            {
                sceneIndex = 0,
                moralityPoints = 50,
                diaryFlags = new int[20],
                playTime = 0f
            };
        }
        else
        {
            Destroy(gameObject);
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


    public void ApplyLoadedProgress(PlayerProgress progress)
    {
        if (progress != null)
        {
            CurrentProgress = progress;

            PlayerPrefs.SetInt("LoadedMorality", progress.moralityPoints);
            PlayerPrefs.SetString("LoadedDiaryFlags", string.Join(",", progress.diaryFlags));
            PlayerPrefs.Save();

            Debug.Log($"Прогресс применен: сцена {progress.sceneIndex}, мораль {progress.moralityPoints}");
        }
    }

}