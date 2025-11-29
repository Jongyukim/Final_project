using UnityEngine;
using System.Collections;

public class FrameActivatedDrawer : MonoBehaviour
{
    [SerializeField] private Transform drawerTransform;
    [SerializeField] private Vector3 openPosition = new Vector3(0.5f, 0, 0);
    [SerializeField] private float openSpeed = 5f;
    [SerializeField] private AudioClip openSound;

    private Vector3 closedPosition;
    private AudioSource audioSource;
    private bool isOpen = false;

    void Start()
    {
        closedPosition = drawerTransform.localPosition;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // GameManager에 자동으로 열릴 서랍 등록
        GameManager.Instance.RegisterAutoDrawer(this);
    }

    public void AutoOpen()
    {
        if (isOpen) return;

        StopAllCoroutines();
        StartCoroutine(OpenDrawer());
    }

    IEnumerator OpenDrawer()
    {
        isOpen = true;

        if (openSound != null)
            audioSource.PlayOneShot(openSound);

        Vector3 targetPos = closedPosition + openPosition;

        while (Vector3.Distance(drawerTransform.localPosition, targetPos) > 0.01f)
        {
            drawerTransform.localPosition =
                Vector3.Lerp(drawerTransform.localPosition, targetPos, Time.deltaTime * openSpeed);

            yield return null;
        }

        drawerTransform.localPosition = targetPos;
    }
}
