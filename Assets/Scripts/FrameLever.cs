using UnityEngine;
using System.Collections;

public class FrameLever : Interactable
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float rotationAngle = 90f;   // Z축 회전하도록 90 정도 추천
    [SerializeField] private float rotationSpeed = 10f;

    private bool isActivated = false;
    private AudioSource audioSource;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        originalRotation = transform.rotation;

        // 세로 → 가로 : Z축 회전
        targetRotation = originalRotation * Quaternion.Euler(0, 0, rotationAngle);
    }

    void Update()
    {
        UpdateInteractText();
    }

    void UpdateInteractText()
    {
        if (!isActivated)
            interactText = "돌리기 (E)";
        else
            interactText = "";
    }

    public override void Interact()
    {
        if (isActivated) return;

        isActivated = true;
        StartCoroutine(RotateFrame());
    }

    IEnumerator RotateFrame()
    {
        PlaySound();

        // 액자 회전
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = targetRotation;

        // GameManager에 알림 → 서랍 자동 오픈
        GameManager.Instance.ActivateFrame();
    }

    void PlaySound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
