using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public Sprite icon;

    public Item(string name, string desc, Sprite iconSprite)
    {
        itemName = name;
        description = desc;
        icon = iconSprite;
    }
}