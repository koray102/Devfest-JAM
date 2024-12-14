using UnityEngine;
using System.Collections.Generic;

public class FiresManager : MonoBehaviour
{
    [Header("Fire-Subduty Associations")]
    public List<FireExtinguisher> fires; // List to store multiple fires

    public DutyManager dutyManager;

    private bool allFiresTriggered = false; // Flag to check if all fires are extinguished

    void Update()
    {
        // If all fires are extinguished, complete the duty
        if (IsAllFiresExtinguished())
        {
            dutyManager.CompleteDuty();
            allFiresTriggered = true; // Prevent re-triggering
        }
    }
       

    // Check if all fires are extinguished
    private bool IsAllFiresExtinguished()
    {
        foreach (FireExtinguisher fire in fires)
        {
            if (!fire.YokOlcak) // If any fire is not extinguished
                return false;
        }
        return true; // All fires are extinguished
    }
}
