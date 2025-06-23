using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Chest : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ChangedVoid()
    {

        if (!open)
        {
            transform.Rotate(Vector3.left * 90);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        }
        else
        {
            transform.Rotate(Vector3.left * 90);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");
            
        }
    }

    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AbrirBau()
    {
        if (!open)
        {
            RPC_ChangedVoid();
            open = true;
        }
    }

}
