using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BookTraduction : NetworkBehaviour, IInteractable
{



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        playerInventory.RPC_AtivarLivro();

    }
}
