using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    private string currentScene;
    private string lastCheckpoint;
    private Vector3 storyPosition;
    private List<bool> journalEntries = new List<bool>();

    public int morality = 50;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMorality(int amount)
    {
        morality += amount;
        morality = Mathf.Clamp(morality, 0, 100);
        Debug.Log("Мораль: " + morality);
    }
}
    /*public void SaveGame()
    {
        // Собираем данные для сохранения
        GameSaveData saveData = new GameSaveData(
            currentScene,
            morality,
            lastCheckpoint,
            storyPosition,
            journalEntries
        );

        StartCoroutine(SaveToDatabase(saveData));
    }

    IEnumerator SaveToDatabase(GameSaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);

        UnityWebRequest request = new UnityWebRequest(serverUrl + "save", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Save failed: " + request.error);
        }
    }
}

[System.Serializable]
public class GameSaveData
{
    public string sceneName;
    public int morality;
    public string lastCheckpoint;
    public float[] storyPosition; // [x, y, z]
    public List<bool> journalData;

    public GameSaveData(string scene, int moral, string checkpoint, Vector3 position, List<bool> journal)
    {
        sceneName = scene;
        morality = moral;
        lastCheckpoint = checkpoint;
        storyPosition = new float[] { position.x, position.y, position.z };
        journalData = journal;
    }
}*/





