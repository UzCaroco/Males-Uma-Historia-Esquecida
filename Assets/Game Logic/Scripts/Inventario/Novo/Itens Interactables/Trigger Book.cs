using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TriggerBook : NetworkBehaviour, IInteractable
{
    [SerializeField] OpenBookshelf openBookshelf;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        openBookshelf.RPC_OpenBookshelf();

        Runner.Despawn(Object); // Despawna o livro
    }
}
