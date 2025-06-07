using Fusion;
using UnityEngine;

public class DoorPadlock : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;
    [Networked][SerializeField] float rotacao { get; set; }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenLock()
    {

    }
}
