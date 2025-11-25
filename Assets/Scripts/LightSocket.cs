using UnityEngine;

public class LightSocket : Interactable
{
    [SerializeField] private Light lightComponent;
    [SerializeField] private InventoryManager inventoryManager;
    private bool isActivated = false;

    void Update()
    {
        CheckBulbAvailability();
    }

    void CheckBulbAvailability()
    {
        bool hasBulb = false;

        if (inventoryManager.GetInventoryCount() > 0)
        {
            Item item = inventoryManager.GetItem(0);
            if (item != null && item.itemName == "전구")
            {
                hasBulb = true;
            }
        }

        if (hasBulb && !isActivated)
        {
            interactText = "끼우기 (E)";
        }
        else if (isActivated)
        {
            interactText = ""; // 이미 끼워짐
        }
        else
        {
            interactText = "";
        }
    }

    public override void Interact()
    {
        if (!isActivated && inventoryManager.GetInventoryCount() > 0)
        {
            Item item = inventoryManager.GetItem(0);

            if (item != null && item.itemName == "전구")
            {
                inventoryManager.RemoveItem(0);

                // 즉시 켜지 않고 GameManager에 등록만
                GameManager.Instance.RegisterLight(lightComponent);

                isActivated = true;
                Destroy(this);
            }
        }
    }
}