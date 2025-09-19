using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;
    public bool isOpeningFlag;
    public bool destroyAfter;
    public int IndexFlag;
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 10)]
    public string text;
    public bool isChoice;
    public Choice[] choices;
    public Sprite characterImage;
    public string characterName;
}

[System.Serializable]
public class Choice
{
    public string text;
    public int moralityEffect;
}