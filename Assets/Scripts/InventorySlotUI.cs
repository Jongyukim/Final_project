using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemNameText;

    public void SetSlot(Item item)
    {
        itemIcon.sprite = item.icon;
        itemIcon.color = Color.white;
        itemNameText.text = item.itemName;
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        itemNameText.text = "";
    }
}
