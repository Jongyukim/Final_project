using UnityEngine;
using System.Collections;

public class CodeDrawer : Interactable
{
    [SerializeField] private Transform drawerTransform;
    [SerializeField] private Vector3 rotationAngle = new Vector3(0, 0, 90f); // Z축 90도 회전
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private Transform codeInputPanel;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        closedRotation = drawerTransform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(rotationAngle);
        UpdateInteractText();
    }

    void Update()
    {
        UpdateInteractText();
    }

    void UpdateInteractText()
    {
        // 순서 확인
        if (!GameProgress.Instance.CanProgressDesk())
        {
            interactText = "";
            return;
        }

        interactText = isOpen ? "닫기 (E)" : "열기 (E)";
    }

    public override void Interact()
    {
        // 순서 확인
        if (!GameProgress.Instance.CanProgressDesk())
        {
            Debug.Log("아직 이 자물쇠를 풀 수 없습니다.");
            return;
        }

        StopAllCoroutines();
        if (!isOpen)
        {
            // 코드 입력 패널 활성화
            if (codeInputPanel != null)
            {
                codeInputPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(CloseDrawer());
        }
    }

    public void TryOpenWithCode(string inputCode)
    {
        if (!isOpen)
        {
            if (inputCode == "295")
            {
                Debug.Log("번호가 맞습니다!");
                StopAllCoroutines();
                StartCoroutine(OpenDrawer());

                // 게임 진행 단계 업데이트
                if (GameProgress.Instance != null)
                {
                    GameProgress.Instance.CompleteDeskPuzzle();
                }
            }
            else
            {
                Debug.Log("번호가 틀렸습니다!");
            }
        }
    }

    IEnumerator OpenDrawer()
    {
        isOpen = true;
        PlaySound();

        while (Quaternion.Angle(drawerTransform.localRotation, openRotation) > 0.1f)
        {
            drawerTransform.localRotation = Quaternion.Lerp(
                drawerTransform.localRotation,
                openRotation,
                Time.deltaTime * rotationSpeed
            );
            yield return null;
        }
        drawerTransform.localRotation = openRotation;
    }

    IEnumerator CloseDrawer()
    {
        isOpen = false;
        PlaySound();

        while (Quaternion.Angle(drawerTransform.localRotation, closedRotation) > 0.1f)
        {
            drawerTransform.localRotation = Quaternion.Lerp(
                drawerTransform.localRotation,
                closedRotation,
                Time.deltaTime * rotationSpeed
            );
            yield return null;
        }
        drawerTransform.localRotation = closedRotation;
    }

    void PlaySound()
    {
        if (openSound != null)
            audioSource.PlayOneShot(openSound);
    }
}