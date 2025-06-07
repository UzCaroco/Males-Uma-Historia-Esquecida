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
            Debug.Log("Invent�rio do jogador n�o � nulo" + playerInventory.Object);
            if (playerInventory.itemAtualID != -1)
            {
                Debug.Log("tem item");
                if (playerInventory.itemAtualID == (int)_data.itemType)
                {
                    Debug.Log("item � igual");
                    transform.Rotate(Vector3.up, 90f); // uso do item

                    playerInventory.RPC_ResetValues();

                    if (doorPadlock != null)
                    {
                        doorPadlock.RPC_OpenLock(); // Chama o RPC para abrir a fechadura
                    }

                }
                else
                {
                    Debug.Log("item � diferente");
                }
            }
            else
            {
                Debug.Log("N�o tem item");
            }
        }
        else
        {
            Debug.Log("Invent�rio do jogador � nulo");
        }
    }
}
