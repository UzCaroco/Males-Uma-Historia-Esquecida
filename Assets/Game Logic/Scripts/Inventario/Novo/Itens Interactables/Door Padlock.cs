using Fusion;
using UnityEngine;

public class DoorPadlock : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;
    [SerializeField] NetworkObject doorLeft, doorRight;


    [Networked][SerializeField] float locksOpens { get; set; }


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
            
            Runner.Despawn(Object); // Despawna a corrente
            doorLeft.transform.Rotate(0, 0, -90);
            doorRight.transform.Rotate(0, 0, 90);
        }
    }
}
