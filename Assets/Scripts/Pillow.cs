using UnityEngine;
using System.Collections;

public class Pillow : Interactable
{
    [SerializeField] private Transform pillowTransform;
    [SerializeField] private GameObject usb;
    [SerializeField] private Vector3 liftPosition = new Vector3(0, 0.5f, 0);
    [SerializeField] private float liftSpeed = 5f;
    [SerializeField] private AudioClip liftSound;

    private Vector3 closedPosition;
    private bool isLifted = false;
    private AudioSource audioSource;

    void Start()
    {
        closedPosition = pillowTransform.localPosition;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        UpdateInteractText();
    }

    void Update()
    {
        UpdateInteractText();
    }

    void UpdateInteractText()
    {
        // 순서 확인
        if (!GameProgress.Instance.CanProgressUSB())
        {
            interactText = "";
            return;
        }

        interactText = isLifted ? "내려놓기 (E)" : "들어올리기 (E)";
    }

    public override void Interact()
    {
        // 순서 확인
        if (!GameProgress.Instance.CanProgressUSB())
        {
            Debug.Log("아직 베개를 들 수 없습니다.");
            return;
        }

        StopAllCoroutines();
        if (!isLifted)
            StartCoroutine(LiftPillow());
        else
            StartCoroutine(LowerPillow());
    }

    IEnumerator LiftPillow()
    {
        isLifted = true;
        usb.SetActive(true);
        PlaySound();
        Vector3 target = closedPosition + liftPosition;
        while (Vector3.Distance(pillowTransform.localPosition, target) > 0.01f)
        {
            pillowTransform.localPosition = Vector3.Lerp(
                pillowTransform.localPosition,
                target,
                Time.deltaTime * liftSpeed
            );
            yield return null;
        }
        pillowTransform.localPosition = target;
    }

    IEnumerator LowerPillow()
    {
        isLifted = false;
        PlaySound();
        Vector3 target = closedPosition;
        while (Vector3.Distance(pillowTransform.localPosition, target) > 0.01f)
        {
            pillowTransform.localPosition = Vector3.Lerp(
                pillowTransform.localPosition,
                target,
                Time.deltaTime * liftSpeed
            );
            yield return null;
        }
        pillowTransform.localPosition = target;
        usb.SetActive(false);
    }

    void PlaySound()
    {
        if (liftSound != null)
            audioSource.PlayOneShot(liftSound);
    }
}