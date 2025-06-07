using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UseItem : NetworkBehaviour, IInteractable
{
    [SerializeField] ItemData _data;

    [SerializeField] Sprite slotVazio;

    [SerializeField] DoorPadlock doorPadlock;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory != null)
        {
            Debug.Log("Inventário do jogador não é nulo" + playerInventory.Object);
            if (playerInventory.itemAtualID != -1)
            {
                Debug.Log("tem item");
                if (playerInventory.itemAtualID == (int)_data.itemType)
                {
                    Debug.Log("item é igual");
                    transform.Rotate(Vector3.up, 90f); // uso do item

                    playerInventory.RPC_ResetValues();

                    if (doorPadlock != null)
                    {
                        doorPadlock.RPC_OpenLock(); // Chama o RPC para abrir a fechadura
                    }

                }
                else
                {
                    Debug.Log("item é diferente");
                }
            }
            else
            {
                Debug.Log("Não tem item");
            }
        }
        else
        {
            Debug.Log("Inventário do jogador é nulo");
        }
    }
}
