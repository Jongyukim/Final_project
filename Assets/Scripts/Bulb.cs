using UnityEngine;

public class Bulb : Interactable
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Sprite bulbIcon;

    public override void Interact()
    {
        Item bulb = new Item("전구", "밝은 전구", bulbIcon);
        inventoryManager.AddItem(bulb);
        Debug.Log("전구를 획득했다!");
        Destroy(gameObject);
    }
}