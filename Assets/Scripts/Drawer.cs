using UnityEngine;
using System.Collections;

public class Drawer : Interactable
{
    [SerializeField] private Transform drawerTransform;
    [SerializeField] private Vector3 openPosition = new Vector3(0.7f, 0, 0);
    [SerializeField] private float openSpeed = 5f;
    [SerializeField] private AudioClip openSound;

    private Vector3 closedPosition;
    private bool isOpen = false;
    private AudioSource audioSource;

    void Start()
    {
        closedPosition = drawerTransform.localPosition;

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
        interactText = isOpen ? "´Ý±â (E)" : "¿­±â (E)";
    }

    public override void Interact()
    {
        StopAllCoroutines();

        if (!isOpen)
            StartCoroutine(OpenDrawer());
        else
            StartCoroutine(CloseDrawer());
    }

    IEnumerator OpenDrawer()
    {
        isOpen = true;
        PlaySound();

        Vector3 target = closedPosition + openPosition;

        while (Vector3.Distance(drawerTransform.localPosition, target) > 0.01f)
        {
            drawerTransform.localPosition = Vector3.Lerp(
                drawerTransform.localPosition,
                target,
                Time.deltaTime * openSpeed
            );
            yield return null;
        }

        drawerTransform.localPosition = target;
    }

    IEnumerator CloseDrawer()
    {
        isOpen = false;
        PlaySound();

        Vector3 target = closedPosition;

        while (Vector3.Distance(drawerTransform.localPosition, target) > 0.01f)
        {
            drawerTransform.localPosition = Vector3.Lerp(
                drawerTransform.localPosition,
                target,
                Time.deltaTime * openSpeed
            );
            yield return null;
        }

        drawerTransform.localPosition = target;
    }

    void PlaySound()
    {
        if (openSound != null)
            audioSource.PlayOneShot(openSound);
    }
}
