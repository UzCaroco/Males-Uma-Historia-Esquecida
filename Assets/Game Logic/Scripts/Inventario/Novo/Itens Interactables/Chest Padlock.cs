using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ChestPadlock : NetworkBehaviour, IInteractable
{
    [Networked] int chestId { get; set; } = 0;
    [Networked] Vector3 chestState { get; set; }
    NetworkBool open = false;

    string codigoCorreto = "OI";


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        playerInventory.RPC_AtivarChestCode();

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_VerificarCodigo(string resposta)
    {
        Debug.Log("CODIGO CHEGOU AQUI" + resposta);

        if (resposta.ToUpper() == codigoCorreto.ToUpper())
        {
            Debug.Log("CÓDIGO CORRETO! ABRINDO O BAÚ...");

        }
        else
        {
            Debug.Log("CÓDIGO INCORRETO! TENTE NOVAMENTE.");
        }
    }
}
