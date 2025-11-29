using UnityEngine;

public class Computer : Interactable
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private AudioClip insertSound; // ← 추가

    private bool isInserted = false;
    private AudioSource audioSource; // ← 추가

    void Start()
    {
        interactText = "";
        Debug.Log("Computer Start!");

        // ← 추가: AudioSource 설정
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        bool canProgress = GameProgress.Instance.CanProgressComputer();
        Debug.Log("Computer Update - CanProgressComputer: " + canProgress);
        if (!canProgress)
        {
            interactText = "";
            return;
        }
        CheckUSBAvailability();
    }

    void CheckUSBAvailability()
    {
        bool hasUSB = false;
        int usbIndex = -1;

        for (int i = 0; i < inventoryManager.GetInventoryCount(); i++)
        {
            Item item = inventoryManager.GetItem(i);
            if (item != null && item.itemName == "USB")
            {
                hasUSB = true;
                usbIndex = i;
                Debug.Log("USB 찾음! Index: " + i);
                break;
            }
        }

        if (hasUSB && !isInserted)
        {
            interactText = "USB 끼우기 (E)";
            Debug.Log("상호작용 텍스트 설정됨!");
        }
        else if (isInserted)
        {
            interactText = "";
        }
        else
        {
            interactText = "";
        }
    }

    public override void Interact()
    {
        if (!GameProgress.Instance.CanProgressComputer())
        {
            Debug.Log("아직 컴퓨터를 사용할 수 없습니다.");
            return;
        }

        int usbIndex = -1;
        for (int i = 0; i < inventoryManager.GetInventoryCount(); i++)
        {
            Item item = inventoryManager.GetItem(i);
            if (item != null && item.itemName == "USB")
            {
                usbIndex = i;
                break;
            }
        }

        if (!isInserted && usbIndex != -1)
        {
            inventoryManager.RemoveItem(usbIndex);
            isInserted = true;

            // ← 추가: 사운드 재생
            PlaySound();

            if (GameProgress.Instance != null)
            {
                GameProgress.Instance.CompleteComputerPuzzle();
            }

            Debug.Log("USB를 컴퓨터에 끼웠다!");
            Destroy(this);
        }
    }

    // ← 추가: 사운드 재생 함수
    void PlaySound()
    {
        if (insertSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(insertSound);
        }
    }
}