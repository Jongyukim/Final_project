using UnityEngine;

public class Bulb : Interactable
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Sprite bulbIcon; // 인벤토리 작은 아이콘
    [SerializeField] private Sprite bulbDetailImage; // 상세보기 큰 이미지

    public override void Interact()
    {
        Item bulb = new Item("전구", "밝은 전구", bulbIcon, bulbDetailImage);
        inventoryManager.AddItem(bulb);
        Debug.Log("전구를 획득했다!");
        Destroy(gameObject);
    }
}