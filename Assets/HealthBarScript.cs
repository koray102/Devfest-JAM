using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBar; // Reference to the health bar UI (Slider)

    public float maxHealth = 100f;
    internal float currentHealth;

    public float smokeDamageRate = 1f;  // Slow damage when smoking
    public float burnDamageRate = 5f;   // Faster damage when burning
    public float bothDamageRate = 10f;  // Even faster damage when both

    public Transform[] restorePoints;
    public float restoreAmount = 20f;

    private bool isSmoking = false;
    private bool isBurning = false;
    private bool isDead = false;

    [SerializeField] private Color burningColor;
    [SerializeField] private Color smokedColor;
    [SerializeField] private Color burnedSmokedColor;
    [SerializeField] private Color healthyColor;

    public float smokingDuration = 5f;
    public float burningDuration = 5f;

    [SerializeField] private VideoPlayer burningVideo; // Burning effect video
    [SerializeField] private VideoPlayer smokingVideo; // Smoking effect video
    public float fadeDuration = 1f; // Duration for the fade effect

    private bool isSmokePlaying = false;
    private bool isBurningPlaying = false;

    // Add references for the sound effects
    public string smokeSound = "smokeSound";
    public string burningSound = "burningSound";
    [SerializeField] private GameObject player;

    private sfxManager SfxManager;

    void Start()
    {
        SfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<sfxManager>();

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        // Ensure the video players are stopped at the start
        if (burningVideo)
        {
            burningVideo.Stop();
            burningVideo.targetCameraAlpha = 0f;
        }
        if (smokingVideo)
        {
            smokingVideo.Stop();
            smokingVideo.targetCameraAlpha = 0f;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.P) && !isBurningPlaying) // Left-click to start burning
        {
            StartCoroutine(BurningEffect());
        }

        if (Input.GetKeyDown(KeyCode.U) && !isSmokePlaying) // Right-click to start smoking
        {
            StartCoroutine(SmokingEffect());
        }

        if (isSmoking && isBurning)
        {
            currentHealth -= bothDamageRate * Time.deltaTime;
            healthBar.fillRect.GetComponent<Image>().color = burnedSmokedColor;
        }
        else if (isSmoking)
        {
            currentHealth -= smokeDamageRate * Time.deltaTime;
            healthBar.fillRect.GetComponent<Image>().color = smokedColor;
        }
        else if (isBurning)
        {
            currentHealth -= burnDamageRate * Time.deltaTime;
            healthBar.fillRect.GetComponent<Image>().color = burningColor;
        }
        else
        {
            healthBar.fillRect.GetComponent<Image>().color = healthyColor;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            HandleDeath();
        }

        foreach (var point in restorePoints)
        {
            if (Vector3.Distance(transform.position, point.position) < 2f)
            {
                RestoreHealth();
                break;
            }
        }
    }

    void HandleDeath()
    {
        Debug.Log("Firefighter is dead!");
    }

    IEnumerator SmokingEffect()
    {
        SetSmokingState(true);

        if (smokingVideo && !smokingVideo.isPlaying)
        {
            smokingVideo.Play();
        }

        SfxManager.PlaySFXOnLoop(smokeSound, player);

        float elapsed = 0f;

        // Fade in effect
        while (elapsed < fadeDuration / 2)
        {
            smokingVideo.targetCameraAlpha = Mathf.Lerp(0f, 0.4f, elapsed / (fadeDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        smokingVideo.targetCameraAlpha = 0.65f;
        yield return new WaitForSeconds(smokingDuration - fadeDuration);

        elapsed = 0f;

        // Fade out effect
        while (elapsed < fadeDuration / 2)
        {
            smokingVideo.targetCameraAlpha = Mathf.Lerp(0.4f, 0f, elapsed / (fadeDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        smokingVideo.targetCameraAlpha = 0f;
        smokingVideo.Stop();

        SfxManager.StopSFX(smokeSound, player); // Ensure this stops properly
        SetSmokingState(false);
    }

    IEnumerator BurningEffect()
    {
        SetBurningState(true);

        if (burningVideo && !burningVideo.isPlaying)
        {
            burningVideo.Play();
        }

        SfxManager.PlaySFXOnLoop(burningSound, player);

        float elapsed = 0f;

        // Fade in effect
        while (elapsed < fadeDuration / 2)
        {
            burningVideo.targetCameraAlpha = Mathf.Lerp(0f, 0.5f, elapsed / (fadeDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        burningVideo.targetCameraAlpha = 0.65f;
        yield return new WaitForSeconds(burningDuration - fadeDuration);

        elapsed = 0f;

        // Fade out effect
        while (elapsed < fadeDuration / 2)
        {
            burningVideo.targetCameraAlpha = Mathf.Lerp(0.5f, 0f, elapsed / (fadeDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        burningVideo.targetCameraAlpha = 0f;
        burningVideo.Stop();

        SfxManager.StopSFX(burningSound, player); // Ensure this stops properly
        SetBurningState(false);
    }


    public void SetSmokingState(bool isSmoking)
    {
        this.isSmoking = isSmoking;
    }

    public void SetBurningState(bool isBurning)
    {
        this.isBurning = isBurning;
    }

    void RestoreHealth()
    {
        currentHealth += restoreAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }
}
