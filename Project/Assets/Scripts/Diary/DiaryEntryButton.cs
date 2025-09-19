using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryEntryButton : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image lockIcon;

    private DiaryEntry entry;
    private DiaryUIManager manager;

    public void Initialize(DiaryEntry diaryEntry, DiaryUIManager uiManager)
    {
        entry = diaryEntry;
        manager = uiManager;

        titleText.text = entry.title;
        lockIcon.gameObject.SetActive(!entry.isUnlocked);

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        manager.DisplayEntry(entry);
    }
}
