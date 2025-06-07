using UnityEngine;

[CreateAssetMenu(fileName = "NovoItem", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public int id;
    public Sprite icon;
    public GameObject itemPrefab;
    public Vector3 position;
}