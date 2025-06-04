using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData itemData;
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory != null)
        {
            if (playerInventory.itemAtual == null)
            {
                playerInventory.itemAtual = itemData;
                playerInventory.itemIcon.sprite = itemData.icon; // Atualiza o �cone do item
                playerInventory.dropPoint = transform; // Armazena a posi��o do item

                Destroy(gameObject); // Destroi o objeto do mundo
            }
            else
            {
                Debug.Log("Invent�rio Cheio");
            }
        }
        else
        {
            Debug.Log("PlayerInventory n�o est� definido.");
        }
    }
}
