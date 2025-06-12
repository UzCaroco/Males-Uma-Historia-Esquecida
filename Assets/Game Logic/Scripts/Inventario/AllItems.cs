using UnityEngine;

public enum ItemType
{
    Default,
    LivroTrigger,
    Isqueiro,
    ChaveSaida,
    ChaveQuarto,
    LamparinaInterativa,
    EDeHabra,
    Tepetes,
    Paes,
    Castanhas
}

[System.Serializable]
public class Item
{
    public string name;
    public ItemType type;
    public short id;
    public Sprite icon;
    public GameObject itemObject;
    public Vector3 position;
}