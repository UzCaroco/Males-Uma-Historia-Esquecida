using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private short id;
    [SerializeField] private Sprite icon;

    Item thisItem;
    Inventory inventoryPlayer;

    private void Start()
    {
        thisItem = new Item(itemName, itemType, id, icon);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryPlayer = other.GetComponent<Inventory>();
            inventoryPlayer.PegarItem(thisItem);
            Debug.Log("Item pego!");
            
            Destroy(gameObject);
        }
    }
}
