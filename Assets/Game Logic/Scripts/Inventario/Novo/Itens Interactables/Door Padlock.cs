using System.Collections;
using Fusion;
using UnityEngine;

public class DoorPadlock : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;
    [SerializeField] NetworkObject doorLeft, doorRight;


    [Networked][SerializeField] int locksOpens { get; set; }

    public override void Spawned()
    {

    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenLock()
    {
        Debug.Log("Tentando abrir a porta com a fechadura");
        if (locksOpens < 2)
        {
            locksOpens++;
        }
        else
        {
            open = true;


            doorLeft.transform.Rotate(0, 0, -120f);
            doorRight.transform.Rotate(0, 0, 120f);

            Runner.Despawn(Object); // Despawna a corrente e o cadeado
        }
    }
}
