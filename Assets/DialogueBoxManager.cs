using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueBoxManager : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Dialogue Content")]
    public List<List<string>> dialogues; // List of dialogue lists
    private int currentDialogueSetIndex = 0;
    private int currentDialogueIndex = 0;

    [Header("References")]
    public DutyManager dutyManager;
    public float typingSpeed = 0.05f;

    private bool isDialogueComplete = false;
    private bool isSentenceFullyTyped = false;

    void Start()
    {
        InitializeDialogues();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isSentenceFullyTyped)
        {
            AdvanceDialogue();
        }
    }

    private void InitializeDialogues()
    {
        // Example dialogue initialization
        dialogues = new List<List<string>>()
        {
            new List<string> { "Welcome to your mission!", "Your first task awaits you." },
            new List<string> { "Great job completing the first task.", "Now, proceed to the next objective." },
            new List<string> { "You've completed all objectives. Well done!" }
        };
    }

    private void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        currentDialogueIndex = 0;
        isDialogueComplete = false;
        StartCoroutine(TypeSentence(dialogues[currentDialogueSetIndex][currentDialogueIndex]));
    }

    private void AdvanceDialogue()
    {
        if (isDialogueComplete) return;

        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues[currentDialogueSetIndex].Count)
        {
            StartCoroutine(TypeSentence(dialogues[currentDialogueSetIndex][currentDialogueIndex]));
        }
        else
        {
            isDialogueComplete = true;
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dutyManager.TriggerDuty(currentDialogueSetIndex);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isSentenceFullyTyped = false;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isSentenceFullyTyped = true;
    }

    public void OnDutyComplete()
    {
        if (currentDialogueSetIndex + 1 < dialogues.Count)
        {
            currentDialogueSetIndex++;
            StartDialogue();
        }
        else
        {
            Debug.Log("All dialogues and duties completed.");
        }
    }
}
