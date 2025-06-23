using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ChestPadlock : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;

    [SerializeField] string codigoCorreto = "OI";

    [SerializeField] Chest chest;


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (!open)
        {
            playerInventory.RPC_AtivarChestCode();
        }

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_VerificarCodigo(string resposta)
    {
        Debug.Log("CODIGO CHEGOU AQUI" + resposta);

        if (resposta.ToUpper() == codigoCorreto.ToUpper())
        {
            Debug.Log("C�DIGO CORRETO! ABRINDO O BA�...");
            if (chest != null && !open)
            {
                chest.RPC_AbrirBau(); // Chama o RPC para abrir o ba�
                open = true; // Marca o ba� como aberto
            }
        }
        else
        {
            Debug.Log("C�DIGO INCORRETO! TENTE NOVAMENTE.");
        }
    }
}
