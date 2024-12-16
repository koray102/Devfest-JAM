using UnityEngine;

public class FiresManager : MonoBehaviour
{
    [Header("Fire-Subduty Associations")]
    public FireExtinguisher fire1;
    private int subdutyCt = 0;

    public DutyManager dutyManager;


    private bool fire1Triggered = false;


    void Update()
    {
        if (subdutyCt == 1)
        {
            dutyManager.CompleteDuty();
        }
        
        if (!fire1Triggered && fire1.YokOlcak)
        {
            fire1Triggered = true;
            subdutyCt++;// Prevent re-triggering
        }

        
    }
}
