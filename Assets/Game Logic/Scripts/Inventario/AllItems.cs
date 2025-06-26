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
    Castanhas,

    ChaveE01,
    ChaveE02,
    ChaveE03,
    ChaveD01,
    ChaveD02,
    ChaveD03,
    Mosquete,
    LivrosAlcorao,
    Agua
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