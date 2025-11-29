using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HorrorGlitchTitle : MonoBehaviour
{
    [Header("Glitch Settings")]
    [SerializeField] private float glitchIntensity = 8f;
    [SerializeField] private float minGlitchInterval = 2.0f;
    [SerializeField] private float maxGlitchInterval = 4.0f;
    [SerializeField] private float glitchDuration = 1.2f;  
    [SerializeField] private bool enableTextDistortion = true;
    [SerializeField] private bool enableColorFlicker = true;
    [SerializeField] private bool enableRandomAppear = true;

    private TextMeshProUGUI titleText;
    private Vector3 originalPos;
    private Color originalColor;
    private string originalText;
    private float nextGlitchTime = 0f;
    private bool isGlitching = false;
    private float glitchEndTime = 0f;

    void Start()
    {
        titleText = GetComponent<TextMeshProUGUI>();

        if (titleText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on " + gameObject.name);
            enabled = false;
            return;
        }

        originalPos = transform.localPosition;
        originalColor = titleText.color;
        originalText = titleText.text;

        if (string.IsNullOrEmpty(originalText))
        {
            Debug.LogWarning("Text is empty on " + gameObject.name);
        }

        nextGlitchTime = Time.time + Random.Range(minGlitchInterval, maxGlitchInterval);
    }

    void Update()
    {
        if (titleText == null) return;

        if (!isGlitching && Time.time >= nextGlitchTime)
        {
            isGlitching = true;
            glitchEndTime = Time.time + glitchDuration;
        }

        if (isGlitching)
        {
            if (Time.time < glitchEndTime)
            {
                ApplyGlitch();
            }
            else
            {
                ResetToNormal();
                isGlitching = false;
                nextGlitchTime = Time.time + Random.Range(minGlitchInterval, maxGlitchInterval);
            }
        }
    }

    void ApplyGlitch()
    {
        if (titleText == null) return;

        float offsetX = Random.Range(-glitchIntensity, glitchIntensity);
        float offsetY = Random.Range(-glitchIntensity, glitchIntensity);
        transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

        if (enableColorFlicker && Random.value < 0.3f)
        {
            if (Random.value < 0.5f)
            {
                titleText.color = new Color(1f, 0f, 0f, 1f);
            }
            else
            {
                titleText.color = originalColor;
            }
        }

        if (enableTextDistortion && Random.value < 0.2f && !string.IsNullOrEmpty(originalText))
        {
            titleText.text = CorruptText(originalText);
        }

        if (enableRandomAppear && Random.value < 0.15f)
        {
            titleText.enabled = Random.value > 0.5f;
        }
    }

    void ResetToNormal()
    {
        if (titleText == null) return;

        transform.localPosition = originalPos;
        titleText.color = originalColor;
        titleText.text = originalText;
        titleText.enabled = true;
    }

    string CorruptText(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        char[] chars = text.ToCharArray();
        int corruptCount = Random.Range(2, Mathf.Min(5, chars.Length));

        for (int i = 0; i < corruptCount; i++)
        {
            int index = Random.Range(0, chars.Length);
            char[] glitchChars = { '█', '▓', '▒', '░', '■', '□', '▪', '▫', '●', '○', '◘', '◙', '◊' };
            chars[index] = glitchChars[Random.Range(0, glitchChars.Length)];
        }

        return new string(chars);
    }
}
