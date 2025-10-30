using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance;
    public static DiaryUIManager diaryUIManager;
    public int[] flags = new int[20];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public int[] GetFlags()
    {
        
        return flags;
    }

    public void UnlockFlag(int index)
    {
        if (index >= 0 && index < flags.Length)
        {
            flags[index] = 1;
            UpdateDiaryEntriesFromFlags();
        }
        
    }

    public void SetFlags(int[] flag)
    {
        for(int i = 0; i < flag.Length; i++)
        {
            flags[i] = (flag[i]);
        }
        UpdateDiaryEntriesFromFlags();
    }

    public void UnlockEntry(int entryId)
    {
        var entry = diaryUIManager?.entries?.Find(e => e.id == entryId);
        if (entry != null)
        {
            entry.isUnlocked = true;
            if (entryId >= 0 && entryId < flags.Length)
            {
                flags[entryId] = 1;
            }
            diaryUIManager?.LoadDiaryEntries();
        }
    }

    public void RegisterDiaryUIManager(DiaryUIManager uiManager)
    {
        diaryUIManager = uiManager;
        if (uiManager != null)
        {
            UpdateDiaryEntriesFromFlags();
        }
    }

    public void UpdateDiaryEntriesFromFlags()
    {
        if (diaryUIManager != null && diaryUIManager.entries != null)
        {
            for (int i = 0; i < flags.Length && i < diaryUIManager.entries.Count; i++)
            {
                diaryUIManager.entries[i].isUnlocked = flags[i] == 1;
            }
            diaryUIManager.LoadDiaryEntries();
        }
    }
}
