using UnityEngine;
using System.Collections;

public class FrameLever : Interactable
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float rotationAngle = 90f;
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
        targetRotation = originalRotation * Quaternion.Euler(0, 0, rotationAngle);
    }

    void Update()
    {
        UpdateInteractText();
    }

    void UpdateInteractText()
    {
        // 순서 확인
        if (!GameProgress.Instance.CanProgressLever())
        {
            interactText = "";
            return;
        }

        if (!isActivated)
            interactText = "돌리기 (E)";
        else
            interactText = "";
    }

    public override void Interact()
    {
        // 순서 확인
        if (!GameProgress.Instance.CanProgressLever())
        {
            Debug.Log("아직 이 퍼즐을 풀 수 없습니다.");
            return;
        }

        if (isActivated) return;
        StartCoroutine(RotateFrame());
    }

    IEnumerator RotateFrame()
    {
        isActivated = true;
        PlaySound();

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = targetRotation;

        GameManager.Instance.ActivateFrame();

        // 포스트잇 활성화 (태그 사용)
        ActivatePostits();

        if (GameProgress.Instance != null)
        {
            GameProgress.Instance.CompleteLeverPuzzle();
        }
    }

    void ActivatePostits()
    {
        // 비활성화된 오브젝트도 포함해서 찾기 (true 파라미터)
        Note[] allNotes = FindObjectsOfType<Note>(true);
        Debug.Log("찾은 포스트잇: " + allNotes.Length);

        foreach (Note note in allNotes)
        {
            Debug.Log("활성화: " + note.gameObject.name);
            note.gameObject.SetActive(true);
        }
    }

    void PlaySound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}