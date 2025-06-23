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
            Debug.Log("CÓDIGO CORRETO! ABRINDO O BAÚ...");
            if (chest != null && !open)
            {
                chest.RPC_AbrirBau(); // Chama o RPC para abrir o baú
                open = true; // Marca o baú como aberto
            }
        }
        else
        {
            Debug.Log("CÓDIGO INCORRETO! TENTE NOVAMENTE.");
        }
    }
}
