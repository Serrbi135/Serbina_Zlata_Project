using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class DiaryUIManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject entryButtonPrefab;
    public Transform contentPanel;
    public Image displayImage;
    public TMP_Text displayText;
    public GameObject lockedInfoPanel;

    [Header("Diary Entries")]
    public List<DiaryEntry> entries;

    private void Start()
    {
        LoadDiaryEntries();
    }

    public void LoadDiaryEntries()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < entries.Count; i++)
        {
            if(DiaryManager.Instance.flags[i] != 0)
            {
                entries[i].isUnlocked = true;
            }
            
        }

        foreach (var entry in entries)
        {
            GameObject buttonObj = Instantiate(entryButtonPrefab, contentPanel);
            DiaryEntryButton button = buttonObj.GetComponent<DiaryEntryButton>();

            button.Initialize(entry, this);
        }
    }

    public void ExitButton()
    {
        Loader.LoadScene("LoadScene");
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
