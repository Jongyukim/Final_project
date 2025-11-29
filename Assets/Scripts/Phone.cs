using UnityEngine;

public class Phone : Interactable
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Sprite phoneIcon;
    [SerializeField] private Sprite phoneDetailImage; // 핸드폰 프레임 이미지

    public override void Interact()
    {
        Item phone = new Item("핸드폰", "스마트폰", phoneIcon, phoneDetailImage);
        inventoryManager.AddItem(phone);

        Debug.Log("핸드폰을 획득했다!");
        Destroy(gameObject);
    }
}