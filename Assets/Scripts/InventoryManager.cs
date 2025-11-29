using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxSlots = 6;
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private Transform detailPanel_Bulb;
    [SerializeField] private Transform detailPanel_Note;
    [SerializeField] private Transform detailPanel_Phone;
    [SerializeField] private Transform codeInputPanel;

    [SerializeField] private TextMeshProUGUI slotIndicator;
    public List<Item> GetInventory() => inventory;
    private List<Item> inventory = new List<Item>();
    private List<Image> slotImages = new List<Image>();
    private int currentSelectedSlot = 0;
    private bool detailPanelOpen = false;

    // ← 추가: 텍스트 숨기기 타이머
    private float hideTextTimer = 0f;
    private const float HIDE_TEXT_DELAY = 3f;

    void Start()
    {
        int slotIndex = 0;
        foreach (Transform slot in inventoryPanel)
        {
            Image itemIcon = slot.Find("ItemIcon").GetComponent<Image>();
            slotImages.Add(itemIcon);
            itemIcon.sprite = null;
            slotIndex++;
        }

        detailPanel_Note.gameObject.SetActive(false);
        detailPanel_Bulb.gameObject.SetActive(false);

        UpdateSlotIndicator();
    }

    void Update()
    {
        HandleMouseWheelScroll();
        HandleQKeyPress();
        UpdateSlotIndicatorTimer(); // ← 추가
    }

    void HandleMouseWheelScroll()
    {
        if (detailPanelOpen) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentSelectedSlot--;
            if (currentSelectedSlot < 0)
                currentSelectedSlot = maxSlots - 1;
            UpdateSlotIndicator();
        }
        else if (scroll < 0f)
        {
            currentSelectedSlot++;
            if (currentSelectedSlot >= maxSlots)
                currentSelectedSlot = 0;
            UpdateSlotIndicator();
        }
    }

    void UpdateSlotIndicator()
    {
        if (currentSelectedSlot < inventory.Count && inventory[currentSelectedSlot] != null)
        {
            slotIndicator.text = inventory[currentSelectedSlot].itemName;
            hideTextTimer = 0f; // ← 타이머 리셋
        }
        else
        {
            slotIndicator.text = "";
        }
    }

    // ← 추가: 타이머 업데이트
    void UpdateSlotIndicatorTimer()
    {
        if (slotIndicator.text != "")
        {
            hideTextTimer += Time.deltaTime;
            if (hideTextTimer >= HIDE_TEXT_DELAY)
            {
                slotIndicator.text = "";
            }
        }
    }

    void HandleQKeyPress()
    {
        // 코드 입력 UI가 활성화되면 인벤토리 무시
        if (codeInputPanel != null && codeInputPanel.gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (detailPanelOpen)
            {
                HideItemDetail();
            }
            else
            {
                if (currentSelectedSlot < inventory.Count && inventory[currentSelectedSlot] != null)
                {
                    ShowItemDetail(inventory[currentSelectedSlot]);
                }
            }
        }
    }

    void ShowItemDetail(Item item)
    {
        if (item == null)
            return;

        Transform detailPanel = null;

        if (item.itemName.Contains("포스트잇"))
        {
            detailPanel = detailPanel_Note;
        }
        else if (item.itemName == "전구")
        {
            detailPanel = detailPanel_Bulb;
        }
        else if (item.itemName == "핸드폰")
        {
            detailPanel = detailPanel_Phone;
        }

        if (detailPanel != null)
        {
            detailPanel.gameObject.SetActive(true);
            detailPanelOpen = true;

            Image detailImage = detailPanel.Find("ItemImage").GetComponent<Image>();
            if (detailImage != null && item.detailImage != null)
            {
                detailImage.sprite = item.detailImage;
            }
        }
    }

    void HideItemDetail()
    {
        detailPanel_Note.gameObject.SetActive(false);
        detailPanel_Bulb.gameObject.SetActive(false);
        detailPanel_Phone.gameObject.SetActive(false);
        detailPanelOpen = false;
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