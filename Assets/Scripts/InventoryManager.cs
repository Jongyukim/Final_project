using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxSlots = 6;
    [SerializeField] private Transform inventoryPanel;

    private List<Item> inventory = new List<Item>();
    private List<Image> slotImages = new List<Image>();

    void Start()
    {
        // 모든 슬롯의 ItemIcon Image 찾기
        int slotIndex = 1;
        foreach (Transform slot in inventoryPanel)
        {
            Image itemIcon = slot.Find("ItemIcon").GetComponent<Image>();
            slotImages.Add(itemIcon);
            itemIcon.sprite = null; // 초기 상태 비움
            slotIndex++;
        }
    }

    public void AddItem(Item item)
    {
        if (inventory.Count < maxSlots)
        {
            inventory.Add(item);
            UpdateInventoryUI();
            Debug.Log(item.itemName + " 획득!");
        }
        else
        {
            Debug.Log("인벤토리가 가득 찼습니다!");
        }
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < inventory.Count)
            {
                slotImages[i].sprite = inventory[i].icon;
            }
            else
            {
                slotImages[i].sprite = null;
            }
        }
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < inventory.Count)
        {
            inventory.RemoveAt(slotIndex);
            UpdateInventoryUI();
        }
    }

    public Item GetItem(int slotIndex)
    {
        if (slotIndex < inventory.Count)
        {
            return inventory[slotIndex];
        }
        return null;
    }

    public int GetInventoryCount()
    {
        return inventory.Count;
    }
}