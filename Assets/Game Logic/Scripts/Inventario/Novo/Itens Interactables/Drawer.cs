using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Drawer : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        Debug.Log("Interagindo com o gaveta");

        if (!open)
        {
            open = true;
            transform.localPosition += new Vector3(0, 0, 0.5f); // Move a gaveta para fora
            Debug.Log("Gaveta aberta com sucesso!" + open);
        }
        else
        {
            open = false;
            transform.localPosition += new Vector3(0, 0, -0.5f); // Move a gaveta de volta
            Debug.Log("Gaveta fechada com sucesso!" + open);
        }
    }


}
