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

    public int[] GetFlags() => flags;
    public void UnlockFlag(int index) => flags[index] = 1;

    public void SetFlags(int[] flag)
    {
        for(int i = 0; i < flag.Length; i++)
        {
            flags[i] = (flag[i]);
        }
    }

    public void UnlockEntry(int entryId)
    {
        // Находим запись и разблокируем
        var entry = diaryUIManager.entries.Find(e => e.id == entryId);
        if (entry != null)
        {
            entry.isUnlocked = true;
            diaryUIManager.LoadDiaryEntries();
        }
    }
}
