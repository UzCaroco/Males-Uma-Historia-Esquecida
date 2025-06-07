using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class OpenBookshelf : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenBookshelf()
    {
        transform.position += new Vector3(2, 0, 0);
    }
}
