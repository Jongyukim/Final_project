using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayDistance = 100f;
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

            if (interactable != null && Vector3.Distance(transform.position, hit.point) <= interactable.InteractDistance)
            {
                // 상호작용 가능
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    ShowInteractUI(interactable.InteractText);
                }
            }
            else
            {
                // 거리 초과 또는 Interactable 없음
                HideInteractUI();
                currentInteractable = null;
            }
        }
        else
        {
            // 아무것도 바라보지 않음
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
        interactText.text = text;
        interactText.gameObject.SetActive(true);
    }

    void HideInteractUI()
    {
        interactText.gameObject.SetActive(false);
    }
}