using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Chest : NetworkBehaviour, IInteractable
{
    [Networked] int chestId { get; set; } = 0;
    [Networked] Vector3 chestState { get; set; }
    NetworkBool open = false;


    public override void FixedUpdateNetwork()
    {
        
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ChangedVoid()
    {
        Debug.Log("Chest state changed: " + chestState);

        if (!open)
        {
            Debug.Log(chestState);

            transform.Rotate(chestState);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        }
        else
        {
            Debug.Log(chestState);

            transform.Rotate(chestState);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");
            
        }
    }

    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        Debug.Log("Interagindo com o porta do baú");

        if (!open)
        {
            chestState = new Vector3(-90, 0, 0);
            open = true;

            if (HasStateAuthority)
                RPC_ChangedVoid();

            chestId++;
            Debug.Log("ID DO BAU " + chestId);
        }
        else
        {
            chestState = new Vector3(90, 0, 0);
            open = false;

            if (HasStateAuthority)
                RPC_ChangedVoid();

            chestId++;
            Debug.Log("ID DO BAU " + chestId);
        }

    }
}
