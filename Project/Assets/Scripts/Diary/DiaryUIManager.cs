using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using DG.Tweening;

public class DiaryUIManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject entryButtonPrefab;
    public Transform contentPanel;
    public Image displayImage;
    public TMP_Text displayText;
    public GameObject lockedInfoPanel;

    public CanvasGroup diaryUIGame;

    [Header("Diary Entries")]
    public List<DiaryEntry> entries;

    private void Start()
    {
        if (DiaryManager.Instance != null)
        {
            DiaryManager.Instance.RegisterDiaryUIManager(this);
        }
        LoadDiaryEntries();
    }

    public void LoadDiaryEntries()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        if (DiaryManager.Instance != null)
        {
            var flags = DiaryManager.Instance.GetFlags();
            for (int i = 0; i < entries.Count && i < flags.Length; i++)
            {
                entries[i].isUnlocked = flags[i] == 1;
            }
        }

        foreach (var entry in entries)
        {
            GameObject buttonObj = Instantiate(entryButtonPrefab, contentPanel);
            DiaryEntryButton button = buttonObj.GetComponent<DiaryEntryButton>();
            button.Initialize(entry, this);
        }
    }

    public void RefreshDiaryWithFlags(int[] flags)
    {
        if (flags == null) return;

        for (int i = 0; i < entries.Count && i < flags.Length; i++)
        {
            entries[i].isUnlocked = flags[i] == 1;
        }

        LoadDiaryEntries(); 
    }

    public void ExitButton()
    {
        Loader.LoadScene("LoadScene");
    }

    public void ExitToGame()
    {
        diaryUIGame.interactable = false;
        diaryUIGame.blocksRaycasts = false;
        diaryUIGame.alpha = 0f;
    }

    public void DisplayEntry(DiaryEntry entry)
    {
        if (entry.isUnlocked)
        {
            displayImage.sprite = entry.image;
            displayText.text = entry.description;
            lockedInfoPanel.SetActive(false);
            displayImage.gameObject.SetActive(true);
        }
        else
        {
            lockedInfoPanel.SetActive(true);
            displayImage.gameObject.SetActive(false);
            displayText.text = "Вы ещё не знаете об этом.";
        }
    }
}
