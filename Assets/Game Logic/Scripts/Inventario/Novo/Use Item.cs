using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UseItem : NetworkBehaviour, IInteractable
{
    [SerializeField] ItemData _data;

    [SerializeField] Sprite slotVazio;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory != null)
        {
            if (playerInventory.itemAtual != null)
            {
                if (playerInventory.itemAtual == _data)
                {
                    transform.Rotate(Vector3.up, 90f); // uso do item
                    playerInventory.itemAtual = null; // Limpa o item atual
                    playerInventory.cam.GetComponent<FirstPersonCamera>().slotItem.sprite = null; // Limpa o ícone do item na câmera
                }
            }
        }
    }
}
