using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiaryEntry
{
    public int id;
    public string title;
    [TextArea(3, 10)] public string description;
    public Sprite image;
    public bool isUnlocked;
}
