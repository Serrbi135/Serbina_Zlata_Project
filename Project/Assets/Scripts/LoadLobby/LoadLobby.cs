using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.SearchService;

public class LoadLobby : MonoBehaviour
{

    public TextMeshProUGUI txtBar;
    public int sceneID;
    public Image LoadBar;
    AsyncOperation asyncOper;

    void Start()
    {
        StartCoroutine(LoadSceneCor());
    }

    public IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        asyncOper = SceneManager.LoadSceneAsync(sceneID);
        while(!asyncOper.isDone)
        {
            float progress = asyncOper.progress / 0.9f;
            LoadBar.fillAmount = progress;
            txtBar.text = "Загрузка..." + string.Format("{0:0}%", progress * 100f);
            yield return 0;

        }

    }
}
