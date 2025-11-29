using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayDistance = 100f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI interactText;

    private Interactable currentInteractable;

    void Update()
    {
        CheckForInteractable();
        HandleInteraction();
    }

    void CheckForInteractable()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            float maxDistance = interactable != null ? interactable.InteractDistance * 1.5f : 3f;

            if (interactable != null && Vector3.Distance(transform.position, hit.point) <= maxDistance)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    ShowInteractUI(interactable.InteractText);
                }
            }
            else
            {
                HideInteractUI();
                currentInteractable = null;
            }
        }
        else
        {
            HideInteractUI();
            currentInteractable = null;
        }
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
            HideInteractUI();
        }
    }

    void ShowInteractUI(string text)
    {
        if (interactText != null)
        {
            interactText.text = text;
            interactText.gameObject.SetActive(true);
        }
    }

    void HideInteractUI()
    {
        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }
    void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 5f);
        }
    }
}