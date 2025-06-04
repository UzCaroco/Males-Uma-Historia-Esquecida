using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData _data;
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory != null)
        {
            if (playerInventory.itemAtual != null)
            {
                if (playerInventory.itemAtual == _data)
                {
                    Destroy(gameObject);
                    playerInventory.itemAtual = null; // Limpa o item atual
                    playerInventory.itemIcon.sprite = null; // Limpa o ícone do item
                }
            }
        }
    }
}
