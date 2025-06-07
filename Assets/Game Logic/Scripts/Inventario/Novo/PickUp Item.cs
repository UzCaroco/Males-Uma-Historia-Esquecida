using Fusion;
using UnityEngine;

public class PickUpItem : NetworkBehaviour, IInteractable
{
    public ItemData itemData;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        Runner.Despawn(Object); // Despawns the network object
    }
}
