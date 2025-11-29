using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public Sprite icon; // 인벤토리 작은 아이콘
    public Sprite detailImage; // Q키 상세보기 큰 이미지

    public Item(string name, string desc, Sprite iconSprite, Sprite detailSprite)
    {
        itemName = name;
        description = desc;
        icon = iconSprite;
        detailImage = detailSprite;
    }
}