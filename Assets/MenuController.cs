using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public TMP_Text playText;
    public TMP_Text settingsText;
    public TMP_Text quitText;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject MenuCanvas;

    [SerializeField] private Color hoverColor; // Set this in the Inspector
    private Color playDefaultColor;
    private Color settingsDefaultColor;
    private Color quitDefaultColor;

    void Start()
    {
        // Store the initial colors of the texts
        playDefaultColor = playText.color;
        settingsDefaultColor = settingsText.color;
        quitDefaultColor = quitText.color;
        SettingsCanvas.SetActive(false);
    }

    void Update()
    {
        HandleHover(playText, playDefaultColor, () => OnPlayButtonClicked());
        HandleHover(settingsText, settingsDefaultColor, () => OnSettingsButtonClicked());
        HandleHover(quitText, quitDefaultColor, () => OnQuitButtonClicked());
    }

    void HandleHover(TMP_Text text, Color defaultColor, System.Action onClickAction)
    {
        // Raycast from mouse position
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        Vector2 mousePos = Input.mousePosition;

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos))
        {
            text.color = hoverColor;

            if (Input.GetMouseButtonDown(0)) // Left mouse click
            {
                onClickAction?.Invoke();
            }
        }
        else
        {
            text.color = defaultColor;
        }
    }

    void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene("MainScene"); // Replace "GameScene" with your scene name
    }

    void OnSettingsButtonClicked()
    {
        Debug.Log("Settings button clicked!");
        MenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}
