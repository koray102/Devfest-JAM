using TMPro;
using UnityEngine;

public class DutyManager : MonoBehaviour
{
    [Header("Duty Settings")]
    [Tooltip("List of duties to be entered via the Inspector")]
    public string[] duties; // Duties entered via Inspector
    public GameObject dutyPanel;
    public TextMeshProUGUI dutyText; // Displays current duty

    [Header("References")]
    public DialogueBoxManager dialogueBoxManager;

    private int currentDutyIndex = -1; // Starts before any duty is active
    private bool isDutyActive = false;

    void Start()
    {
        if (duties == null || duties.Length == 0)
        {
            Debug.LogWarning("No duties have been assigned in the Inspector.");
        }
    }

    public void TriggerDuty(int dutyIndex)
    {
        if (duties == null || duties.Length == 0)
        {
            Debug.LogError("No duties available to trigger. Please assign duties in the Inspector.");
            return;
        }

        if (dutyIndex >= duties.Length)
        {
            Debug.LogError("Duty index out of bounds.");
            return;
        }

        currentDutyIndex = dutyIndex;
        isDutyActive = true;

        dutyPanel.SetActive(true);
        dutyText.text = duties[dutyIndex];
    }

    public void CompleteDuty()
    {
        if (!isDutyActive) return;

        Debug.Log($"Duty '{duties[currentDutyIndex]}' completed.");
        isDutyActive = false;
        dutyPanel.SetActive(false);

        // Notify DialogueBoxManager to move to the next dialogue
        dialogueBoxManager.OnDutyComplete();
    }
}
