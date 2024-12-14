using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button backButton;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_Dropdown difficultyDropdown;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;

    [Header("Audio Settings")]
    [SerializeField] private float defaultMasterVolume = 0.8f;
    [SerializeField] private float defaultSFXVolume = 0.8f;

    [Header("Difficulty Settings")]
    [SerializeField] private string[] difficultyLevels = { "Easy", "Medium", "Hard" };
    [SerializeField] private int defaultDifficulty = 1;

    private void Start()
    {
        // Initialize UI elements
        InitializeSettings();

        // Add listeners to UI elements
        backButton.onClick.AddListener(OnBackButtonPressed);
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
    }

    private void InitializeSettings()
    {
        difficultyDropdown.ClearOptions();
        difficultyDropdown.AddOptions(new System.Collections.Generic.List<string>(difficultyLevels));
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", defaultDifficulty);
        Debug.Log($"Loaded Difficulty: {savedDifficulty}");
        difficultyDropdown.value = savedDifficulty;
        difficultyDropdown.RefreshShownValue();

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);
    }


    public void OnBackButtonPressed()
    {
        Debug.Log("Back button pressed!");
        // Save settings before exiting
        SaveSettings();
        // Close settings menu (implement your own UI logic here)
        SettingsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    private void OnMasterVolumeChanged(float value)
    {
        Debug.Log($"Master Volume changed to {value}");
        AudioListener.volume = value; // Adjust overall game volume
    }

    private void OnSFXVolumeChanged(float value)
    {
        Debug.Log($"SFX Volume changed to {value}");
    }

    public void OnDifficultyChanged(int value)
    {
        // Log the change for debugging
        Debug.Log($"Difficulty changed to {difficultyLevels[value]} (index: {value})");

        // Save the selected difficulty immediately
        PlayerPrefs.SetInt("Difficulty", value);
        PlayerPrefs.Save();

        // Optional: Notify other systems in your game about the difficulty change
        // Example: GameManager.Instance.SetDifficulty(value);
    }


    private void SaveSettings()
    {
        // Save the current settings using PlayerPrefs
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetInt("Difficulty", difficultyDropdown.value);
        PlayerPrefs.Save();
    }
}
