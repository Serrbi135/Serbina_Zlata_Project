using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image characterImage;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;
    public GameObject moralityEffectPrefab;
    public GameObject diaryEffectPrefab;



    public event Action OnDialogueEnd;

    [Header("Settings")]
    public float textSpeed = 0.05f;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isTyping;
    private bool isChoiceDialogue;
    private PlayerScriptSujet playerMovement;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerMovement = FindObjectOfType<PlayerScriptSujet>();

        dialoguePanel.SetActive(false);
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        characterImage.sprite = dialogue.characterImage;
        characterNameText.text = dialogue.characterName;
        if (currentDialogue.isOpeningFlag)
        {
            ShowDiaryEffect();
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentDialogue.lines[currentLineIndex - 1].text;
            isTyping = false;
            return;
        }

        if (currentLineIndex < currentDialogue.lines.Length)
        {
            DialogueLine line = currentDialogue.lines[currentLineIndex];

            if(line.characterImage != null) 
            { 
                characterImage.sprite = line.characterImage; 
            }
            else
            {
                characterImage.sprite = currentDialogue.characterImage;
            }

            if (line.characterName != null)
            {
                characterNameText.text = line.characterName;
            }
            else
            {
                characterNameText.text = currentDialogue.characterName;
            }

            if (line.isChoice)
            {
                StartCoroutine(TypeText(line.text));
                ShowChoiceButtons(line);
                isChoiceDialogue = true;
            }
            else
            {
                StartCoroutine(TypeText(line.text));
                currentLineIndex++;
            }
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void ShowChoiceButtons(DialogueLine line)
    {
        for (int i = 0; i < line.choices.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].GetComponentInChildren<TMP_Text>().text = line.choices[i].text;

            int choiceIndex = i; 
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => MakeChoice(line.choices[choiceIndex]));
        }
    }

    private void MakeChoice(Choice choice)
    {
        MoralitySystem.Instance.AddPoints(choice.moralityEffect);

        ShowMoralityEffect(choice.moralityEffect);

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        isChoiceDialogue = false;
        currentLineIndex++;
        DisplayNextLine();
    }

    private void ShowMoralityEffect(int amount)
    {
        GameObject effect = Instantiate(moralityEffectPrefab, dialoguePanel.transform);
        TMP_Text effectText = effect.GetComponent<TMP_Text>();
        CanvasGroup canvasGroup = effect.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1, 1f);
        effectText.text = (amount > 0 ? "общение +" : "учёба +") + Mathf.Abs(amount);
        effectText.color = amount > 0 ? Color.blue : Color.red;

        Destroy(effect, 2f);
    }

    private void ShowDiaryEffect()
    {
        GameObject effect = Instantiate(diaryEffectPrefab, dialoguePanel.transform);
        CanvasGroup canvasGroup = effect.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1, 1f);
        TMP_Text effectText = effect.GetComponent<TMP_Text>();
        effectText.text = "Открыта запись в дневнике!";
        effectText.color = Color.magenta;

        Destroy(effect, 2f);
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        if (currentDialogue.isOpeningFlag)
        {
            DiaryManager.Instance.UnlockFlag(currentDialogue.IndexFlag);
            ShowDiaryEffect();
        }

        currentDialogue = null;
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }
        OnDialogueEnd?.Invoke();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialoguePanel.activeSelf && !isChoiceDialogue)
        {
            DisplayNextLine();
        }
    }
}
