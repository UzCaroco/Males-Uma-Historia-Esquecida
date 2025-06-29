using Fusion;
using UnityEngine;

public class PickUpItem : NetworkBehaviour, IInteractable
{
    public ItemData itemData;
    public override void Spawned()
    {
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        playerInventory.RPC_AtivarSomDePickUp(); // Play pickup sound
        Runner.Despawn(Object); // Despawns the network object
    }
}
