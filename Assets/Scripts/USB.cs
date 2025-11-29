using UnityEngine;

public class USB : Interactable
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Sprite usbIcon;
    [SerializeField] private Sprite usbDetailImage;

    void Start()
    {
        interactText = "줍기 (E)";
    }

    public override void Interact()
    {
        Item usb = new Item("USB", "데이터가 저장된 USB", usbIcon, usbDetailImage);
        inventoryManager.AddItem(usb);
        if (GameProgress.Instance != null)
        {
            GameProgress.Instance.CompleteUSBCollection();
        }
        Destroy(gameObject);
    }
}