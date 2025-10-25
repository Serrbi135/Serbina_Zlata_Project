using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public float interactionDistance = 2f;
    public bool destroyAfterDialogue = false;
    [SerializeField] private AudioClip soundToPlay;

    private bool isDialogueActive = false;

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueEnd += HandleDialogueEnd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange() && !isDialogueActive)
        {
            StartDialogue();
        }
        
    }

    private void StartDialogue()
    {
        DialogueSystem.Instance.OnDialogueEnd += HandleDialogueEnd;
        isDialogueActive = true;
        AudioManager.Instance.PlaySFX(soundToPlay);

        dialogue.destroyAfter = destroyAfterDialogue;
        DialogueSystem.Instance.StartDialogue(dialogue);
    }

    private bool IsPlayerInRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        return Vector3.Distance(transform.position, player.transform.position) <= interactionDistance;
    }

    private void HandleDialogueEnd()
    {
        if (isDialogueActive && destroyAfterDialogue)
        {
            DialogueSystem.Instance.OnDialogueEnd -= HandleDialogueEnd;
            Destroy(gameObject);
        }
        else if (isDialogueActive)
        {
            DialogueSystem.Instance.OnDialogueEnd -= HandleDialogueEnd;
            isDialogueActive = false;
        }
    }

    private void OnDestroy()
    {
        if (DialogueSystem.Instance != null && isDialogueActive)
        {
            DialogueSystem.Instance.OnDialogueEnd -= HandleDialogueEnd;
        }
    }
}

