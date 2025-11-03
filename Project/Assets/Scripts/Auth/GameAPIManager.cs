using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using static GameAPIManager;


public class GameAPIManager : MonoBehaviour
{
    private string baseUrl = "http://localhost:5000";
    private UIManager uiManager;

    private float totalPlayTime = 0f;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Register(string username, string password, System.Action onSuccess)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/register", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<RegisterResponse>(request.downloadHandler.text);
            if (response.status == "success")
            {
                onSuccess?.Invoke();
            }
            else
            {
                uiManager.ShowError(response.message); 
            }
        }
        else
        {
            var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
            uiManager.ShowError(errorResponse.message ?? "Ошибка соединения");
        }
    }

    [System.Serializable]
    public class RegisterResponse
    {
        public string status;
        public string message;
    }

    [System.Serializable]
    public class ErrorResponse
    {
        public string message;
    }

    public void SetTotalPlayTime(float time)
    {
        totalPlayTime = time;
    }

    // Метод для получения общего времени
    public float GetTotalPlayTime()
    {
        return totalPlayTime;
    }

    public IEnumerator Login(string username, string password, System.Action<int> onSuccess)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            if (response.status == "success" && response.user_id > 0) 
            {
                PlayerPrefs.SetInt("CurrentUserId", response.user_id); 
                PlayerPrefs.Save();
                onSuccess?.Invoke(response.user_id);
                yield return LoadTotalPlayTime(response.user_id);
            }
            else
            {
                uiManager.ShowError("Неправильный ID");
            }
        }
        else
        {
            uiManager.ShowError($"Войти не удалось: {request.error}");
        }
    }



    public IEnumerator GetLastSaveInfo(int userId, Action<SaveInfo> callback)
    {
        string jsonData = $"{{\"user_id\":{userId}}}";

        using (UnityWebRequest request = new UnityWebRequest(baseUrl + "/save_info", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<SaveInfoResponse>(request.downloadHandler.text);
                callback?.Invoke(response.info);
            }
        }
    }

    public IEnumerator DeleteSave(int userId, Action onSuccess)
    {
        string jsonData = $"{{\"user_id\":{userId}}}";

        using (UnityWebRequest request = new UnityWebRequest(baseUrl + "/delete_save", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Save deleted successfully on server");
                onSuccess?.Invoke();
            }
            else
            {
                Debug.LogError($"DeleteSave failed: {request.error}");
                onSuccess?.Invoke();
            }
        }
    }



    private class SaveInfoResponse { public SaveInfo info; }
    [System.Serializable]
    public class SaveInfo
    {
        
        public int sceneIndex;
    }

    public IEnumerator SaveGame(int userId, PlayerProgress progress, System.Action<bool> callback)
    {
        if (progress == null)
        {
            Debug.LogError("Progress is null!");
            callback?.Invoke(false);
            yield break;
        }

        var saveData = new SaveRequest
        {
            user_id = userId,
            scene_index = progress.sceneIndex,
            morality = progress.moralityPoints,
            diary_flags = progress.diaryFlags ?? new int[0],
            play_time = totalPlayTime 
        };


        string json = JsonUtility.ToJson(saveData);
        Debug.Log("Sending JSON: " + json);


        using (var request = new UnityWebRequest(baseUrl + "/save", "POST"))
        {
            byte[] body = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Save failed. Error: {request.error}, Response: {request.downloadHandler.text}");
            }
            callback?.Invoke(request.result == UnityWebRequest.Result.Success);
        }
    }


    [System.Serializable]
    private class SaveRequest
    {
        public int user_id;
        public int scene_index;
        public int morality;
        public int[] diary_flags;
        public float play_time;
    }

    private IEnumerator LoadTotalPlayTime(int userId)
    {
        string jsonData = $"{{\"user_id\":{userId}}}";

        using (var request = new UnityWebRequest(baseUrl + "/load", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<LoadResponse>(request.downloadHandler.text);
                    if (response.status == "success")
                    {
                        totalPlayTime = response.play_time;
                        Debug.Log($"Загружено общее время: {totalPlayTime}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка загрузки времени: {e.Message}");
                }
            }
        }
    }



    public IEnumerator LoadGame(int userId, System.Action<PlayerProgress> callback)
    {
        string jsonData = $"{{\"user_id\":{userId}}}";
        Debug.Log($"Отправляемые данные: {jsonData}");

        using (var request = UnityWebRequest.PostWwwForm(baseUrl + "/load", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log($"Статус запроса: {request.result}");
            Debug.Log($"Код ответа: {request.responseCode}");
            Debug.Log($"Ответ сервера: {request.downloadHandler.text}");

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<LoadResponse>(request.downloadHandler.text);
                    Debug.Log($"Полученные данные: Scene={response.scene_index}, Morality={response.morality}, Flags={string.Join(",", response.diary_flags)}, PlayTime={response.play_time}");

                    if (response.status == "success")
                    {
                        totalPlayTime = response.play_time;
                        callback?.Invoke(new PlayerProgress
                        {
                            sceneIndex = response.scene_index,
                            moralityPoints = response.morality,
                            diaryFlags = response.diary_flags,
                            playTime = response.play_time
                        });
                    }
                    else
                    {
                        Debug.LogError($"Ошибка в ответе сервера: {request.downloadHandler.text}");
                        callback?.Invoke(null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка парсинга ответа: {e.Message}");
                    callback?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"Ошибка загрузки: {request.error}");
                callback?.Invoke(null);
            }
        }
    }

    [System.Serializable]
    private class SaveResponse
    {
        public string status;
    }

    [System.Serializable]
    private class LoadResponse
    {
        public string status;
        public int scene_index;
        public int morality;
        public int[] diary_flags;
        public float play_time;
    }


    public IEnumerator LoadAndApplyDiaryFlags(int userId)
    {
        yield return LoadGame(userId, (progress) =>
        {
            if (progress != null && progress.diaryFlags != null)
            {
                DiaryManager.Instance.SetFlags(progress.diaryFlags);

                if (DiaryManager.diaryUIManager != null)
                {
                    for (int i = 0; i < progress.diaryFlags.Length; i++)
                    {
                        if (progress.diaryFlags[i] == 1)
                        {
                            DiaryManager.Instance.UnlockEntry(i);
                        }
                    }
                    DiaryManager.diaryUIManager.LoadDiaryEntries();
                }
                else
                {
                    Debug.LogWarning("DiaryUIManager не найден. UI дневника не будет обновлен.");
                }
            }
            else
            {
                Debug.LogWarning("Не удалось загрузить данные дневника или данные отсутствуют.");
            }
        });
    }
    public IEnumerator CheckSaveExists(int userId, Action<bool> callback)
    {
        string jsonData = $"{{\"user_id\":{userId}}}";

        using (var request = new UnityWebRequest(baseUrl + "/check_save", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<SaveCheckResponse>(request.downloadHandler.text);
                callback?.Invoke(response.exists);
            }
            else
            {
                Debug.LogError($"CheckSaveExists failed: {request.error}");
                callback?.Invoke(false);
            }
        }
    }

    [System.Serializable]
    private class SaveCheckResponse
    {
        public bool exists;
    }

    [System.Serializable]
    private class DeleteSaveResponse
    {
        public string status;
        public string message;
    }


    public IEnumerator GetSaveData(int userId, System.Action<PlayerProgress> callback)
    {
        string jsonData = $"{{\"user_id\":{userId}}}";

        using (var request = UnityWebRequest.PostWwwForm(baseUrl + "/load", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var response = JsonUtility.FromJson<LoadResponse>(request.downloadHandler.text);
                    if (response.status == "success")
                    {
                        callback?.Invoke(new PlayerProgress
                        {
                            sceneIndex = response.scene_index,
                            moralityPoints = response.morality,
                            diaryFlags = response.diary_flags,
                            playTime = response.play_time
                        });
                    }
                    else
                    {
                        callback?.Invoke(null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка парсинга ответа: {e.Message}");
                    callback?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"Ошибка загрузки: {request.error}");
                callback?.Invoke(null);
            }
        }
    }




    [System.Serializable]
    private class LoadRequest
    {
        public int user_id;
    }


    [System.Serializable]
    private class BasicResponse
    {
        public string status;
    }

    
    

    
}





[System.Serializable]
public class LoginResponse
{
    public string status;
    public int user_id;
}