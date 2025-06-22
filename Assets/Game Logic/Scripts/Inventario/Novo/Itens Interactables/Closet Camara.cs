using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ClosetCamara : NetworkBehaviour, IInteractable
{
    NetworkBool temGente = false;

    Vector3 posicaoSaida;

    public override void Spawned()
    {
        posicaoSaida = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (!temGente)
        {
            Debug.Log("Entrando no armário");
            playerInventory.RPC_EntrarNoArmario(transform.position);
            temGente = true;
        }
        else
        {
            Debug.Log("Saindo do armário");
            playerInventory.RPC_SairDoArmario(posicaoSaida);
            temGente = false;
        }
    }
}
