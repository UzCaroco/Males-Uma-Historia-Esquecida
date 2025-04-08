using UnityEngine;

public enum ItemType
{
    Default,
    Martelo,
    Lanterna,
    Chave,
    Tesoura,
    Faca
}

[System.Serializable]
public class Item
{
    public string name;
    public ItemType type;
    public short id;
    public Sprite icon;

    public Item(string name, ItemType type, short id, Sprite icon)
    {
        this.name = name;
        this.type = type;
        this.id = id;
        this.icon = icon;
    }
}