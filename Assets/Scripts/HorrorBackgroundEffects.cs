using UnityEngine;
using UnityEngine.UI;

public class CreepyBackgroundEffects : MonoBehaviour
{
    [Header("Blur/Fade Effects")]
    [SerializeField] private bool enableFade = false;
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private float minFadeAlpha = 0.7f;
    [SerializeField] private float maxFadeAlpha = 1f;

    [Header("Wave/Ripple Effects")]
    [SerializeField] private bool enableWave = false;
    [SerializeField] private float waveSpeed = 1f;
    [SerializeField] private float waveIntensity = 10f;
    [SerializeField] private float waveFrequency = 2f;

    [Header("Breathing Effect")]
    [SerializeField] private bool enableBreathing = false;
    [SerializeField] private float breathSpeed = 0.5f;
    [SerializeField] private float breathIntensity = 0.05f;

    [Header("Distortion Effect")]
    [SerializeField] private bool enableDistortion = false;
    [SerializeField] private float distortionSpeed = 2f;
    [SerializeField] private float distortionAmount = 5f;

    [Header("Vignette Pulse")]
    [SerializeField] private bool enableVignettePulse = false;
    [SerializeField] private float vignettePulseSpeed = 1f;
    [SerializeField] private float minVignetteAlpha = 0.3f;
    [SerializeField] private float maxVignetteAlpha = 0.7f;

    [Header("Random Flicker")]
    [SerializeField] private bool enableRandomFlicker = false;
    [SerializeField] private float flickerChance = 0.05f;
    [SerializeField] private Color flickerColor = new Color(0.3f, 0.1f, 0.1f, 1f);

    [Header("Slow Shake")]
    [SerializeField] private bool enableSlowShake = false;
    [SerializeField] private float slowShakeSpeed = 0.5f;
    [SerializeField] private float slowShakeAmount = 2f;

    private Image backgroundImage;
    private Vector3 originalPos;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        backgroundImage = GetComponent<Image>();

        if (backgroundImage == null)
        {
            Debug.LogError("Image component not found!");
            enabled = false;
            return;
        }

        originalPos = transform.localPosition;
        originalScale = transform.localScale;
        originalColor = backgroundImage.color;
    }

    void Update()
    {
        if (backgroundImage == null) return;

        if (enableFade) ApplyFade();
        if (enableWave) ApplyWave();
        if (enableBreathing) ApplyBreathing();
        if (enableDistortion) ApplyDistortion();
        if (enableVignettePulse) ApplyVignettePulse();
        if (enableRandomFlicker) ApplyRandomFlicker();
        if (enableSlowShake) ApplySlowShake();
    }

    void ApplyFade()
    {
        float alpha = Mathf.Lerp(minFadeAlpha, maxFadeAlpha,
            (Mathf.Sin(Time.time * fadeSpeed) + 1f) / 2f);
        Color color = backgroundImage.color;
        color.a = alpha;
        backgroundImage.color = color;
    }

    void ApplyWave()
    {
        float wave = Mathf.Sin(Time.time * waveSpeed * waveFrequency) * waveIntensity;
        transform.localPosition = originalPos + new Vector3(wave, 0, 0);
    }

    void ApplyBreathing()
    {
        float scale = 1f + Mathf.Sin(Time.time * breathSpeed) * breathIntensity;
        transform.localScale = originalScale * scale;
    }

    void ApplyDistortion()
    {
        float x = Mathf.PerlinNoise(Time.time * distortionSpeed, 0) * distortionAmount - distortionAmount / 2;
        float y = Mathf.PerlinNoise(0, Time.time * distortionSpeed) * distortionAmount - distortionAmount / 2;
        transform.localPosition = originalPos + new Vector3(x, y, 0);
    }

    void ApplyVignettePulse()
    {
        float brightness = Mathf.Lerp(minVignetteAlpha, maxVignetteAlpha,
            (Mathf.Sin(Time.time * vignettePulseSpeed) + 1f) / 2f);
        backgroundImage.color = originalColor * brightness;
    }

    void ApplyRandomFlicker()
    {
        if (Random.value < flickerChance)
        {
            backgroundImage.color = flickerColor;
        }
        else
        {
            backgroundImage.color = originalColor;
        }
    }

    void ApplySlowShake()
    {
        float x = Mathf.Sin(Time.time * slowShakeSpeed) * slowShakeAmount;
        float y = Mathf.Cos(Time.time * slowShakeSpeed * 0.7f) * slowShakeAmount;
        transform.localPosition = originalPos + new Vector3(x, y, 0);
    }
}