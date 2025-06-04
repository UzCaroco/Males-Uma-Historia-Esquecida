using UnityEngine;
using Fusion;

public interface IInteractable
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_OnInteractObject(Inven playerInventory);
}
