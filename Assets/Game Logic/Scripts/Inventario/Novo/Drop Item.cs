using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    public void OnInteractObject(PlayerInventory playerInventory)
    {
        if (playerInventory != null)
        {
            if (playerInventory.itemAtual != null)
            {
                Instantiate(playerInventory.itemAtual.itemPrefab, playerInventory.dropPoint);
                playerInventory.itemAtual = null; // Limpa o item atual
                playerInventory.itemIcon.sprite = null; // Limpa o ícone do item
            }
        }
    }
}
