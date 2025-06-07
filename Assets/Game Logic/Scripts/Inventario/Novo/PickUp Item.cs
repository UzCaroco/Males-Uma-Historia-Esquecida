using System.Collections;
using System.Collections.Generic;
using Fusion;
using JetBrains.Annotations;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        
    }
}
