using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory != null)
        {
            if (playerInventory.itemAtual != null)
            {
                Instantiate(playerInventory.itemAtual.itemPrefab, playerInventory.dropPoint);
                playerInventory.itemAtual = null; // Limpa o item atual
                playerInventory.itemIcon = null; // Limpa o ícone do item
            }
        }
    }
}
