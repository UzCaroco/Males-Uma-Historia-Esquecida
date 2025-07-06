
using Fusion;
using UnityEngine;

public class DoorPrision : NetworkBehaviour, IInteractable
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenDoorPrision()
    {
        Debug.Log("abrindo porta da prisão");
        transform.Rotate(Vector3.forward * 160);
    }
}
