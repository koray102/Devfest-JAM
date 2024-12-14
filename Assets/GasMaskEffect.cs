using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class GasMaskEffect : MonoBehaviour
{
    [SerializeField] private GameObject player; // Serialized field for player reference in the inspector
    private bool wearingGasMask = true;
    private bool isBreathingIn = true; // Track if we're currently breathing in or out
    private sfxManager SfxManager;

    [SerializeField] private HealthBarScript healthBarScript; // Reference to the health bar script


    private float maxPitch = 2.0f; // Maximum pitch when health is at minimum
    private float minPitch = 1.0f; // Normal pitch when health is at maximum
    private float maxVolume = 0.8f;
    private float minVolume = 0.2f;

    void Start()
    {
        // Find the sfxManager in the scene (assuming it's tagged as "SFX")
        SfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<sfxManager>();

        // Start the initial breathing sound (breath in)
        StartBreathingSound();
        SfxManager.PlaySFXOnLoop("heartBeat", player);
        SfxManager.SetFXVolume("heartBeat", player, 0.1f);
    }

    void Update()
    {
        // If wearing gas mask, handle breathing sound toggling
        if (wearingGasMask)
        {
            AdjustBreathingAndHeartBeatPitch(); // Adjust the pitch based on health
            if (isBreathingIn && !SfxManager.IsSFXPlaying("breathIn", player))
            {
                StartBreathingSound();
            }
            else if (!isBreathingIn && !SfxManager.IsSFXPlaying("breathOut", player))
            {
                StartBreathingSound();
            }
        }
    }

    private void AdjustBreathingAndHeartBeatPitch()
    {
        if (healthBarScript == null)
        {
            Debug.LogWarning("HealthBarScript reference is missing!");
            return;
        }

        // Get the current health ratio
        float healthRatio = healthBarScript.currentHealth / healthBarScript.maxHealth;

        // Interpolate pitch based on health (same as before)
        float pitch = Mathf.Lerp(maxPitch, minPitch, healthRatio);

        // Interpolate volume based on health (reverse the interpolation for less volume at higher health)
        float volume = Mathf.Lerp(maxVolume, minVolume, healthRatio);  // Higher health = lower volume

        // Debug logs to verify the values
        Debug.Log("Health Ratio: " + healthRatio);  // Debug health ratio
        Debug.Log("Calculated Pitch: " + pitch);    // Debug pitch value
        Debug.Log("Calculated Volume: " + volume);  // Debug volume value

        // Apply pitch and volume to the SFX
        SfxManager.SetFXPitch("breathIn", player, pitch);
        SfxManager.SetFXPitch("breathOut", player, pitch);
        SfxManager.SetFXVolume("heartBeat", player, volume);
        SfxManager.SetFXVolume("heartBeat", player, volume);
    }

    private void StartBreathingSound()
    {
        if (isBreathingIn)
        {
            if (!SfxManager.IsSFXPlaying("breathIn", player))
            {
                SfxManager.PlaySFXOnLoop("breathIn", player);
            }
        }
        else
        {
            if (!SfxManager.IsSFXPlaying("breathOut", player))
            {
                SfxManager.PlaySFXOnLoop("breathOut", player);
            }
        }
    }

    public void SwitchBreathState()
    {
        if (isBreathingIn)
        {
            SfxManager.StopSFX("breathIn", player);
        }
        else
        {
            SfxManager.StopSFX("breathOut", player);
        }

        isBreathingIn = !isBreathingIn;
        StartBreathingSound();
    }


}
