using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogueCreator : EditorWindow
{
#if UNITY_EDITOR


    [MenuItem("Tools/Dialogue Creator")]
    public static void ShowWindow()
    {
        GetWindow<DialogueCreator>("Dialogue Creator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Create New Dialogue"))
        {
            CreateNewDialogue();
        }
    }

    private void CreateNewDialogue()
    {
        Dialogue newDialogue = ScriptableObject.CreateInstance<Dialogue>();

        string path = EditorUtility.SaveFilePanelInProject(
            "Save New Dialogue",
            "NewDialogue",
            "asset",
            "Please enter a file name to save the dialogue to");

        if (string.IsNullOrEmpty(path)) return;

        AssetDatabase.CreateAsset(newDialogue, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newDialogue;
    }
#endif
}