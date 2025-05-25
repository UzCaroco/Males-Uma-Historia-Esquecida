using UnityEngine;
using Fusion;

public interface IInteractable
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void OnInteractObject(PlayerInventory playerInventory);
}
