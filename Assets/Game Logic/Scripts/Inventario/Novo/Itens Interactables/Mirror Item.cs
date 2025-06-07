using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MirrorItem : NetworkBehaviour, IInteractable
{
    NetworkBool direcao = true;
    [Networked] float rotacao { get; set; } = 0;

    [Networked] float min { get; set; }
    [Networked] float max { get; set; }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ChangedVoid()
    {
        Debug.Log("Interagindo com o espelho");
        if (direcao && rotacao < max)
        {
            transform.Rotate(Vector3.right * 0.5f);
            Debug.Log(rotacao);
        }
        else if (direcao && rotacao >= max)
        {
            direcao = false;
            transform.Rotate(Vector3.left * -0.5f);
        }
        else if (!direcao && rotacao > min)
        {
            transform.Rotate(Vector3.right * -0.5f);
            Debug.Log(rotacao);
        }
        else if (!direcao && rotacao <= min)
        {
            direcao = true;
            transform.Rotate(Vector3.left * 0.5f);
        }

    }


    public override void Spawned()
    {
        rotacao = transform.eulerAngles.x;
        min = rotacao - 80;
        max = rotacao + 80;
        Debug.Log(this.gameObject.transform.parent.name);
        Debug.Log(min + " " + max);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (direcao && rotacao < max)
        {
            rotacao += 1;
            if (HasStateAuthority)
                RPC_ChangedVoid();
        }
        else if (direcao && rotacao >= max)
        {
            rotacao -= 1;
            if (HasStateAuthority)
                RPC_ChangedVoid();
        }
        else if (!direcao && rotacao > min)
        {
            rotacao -= 1;
            if (HasStateAuthority)
                RPC_ChangedVoid();
        }
        else if (!direcao && rotacao <= min)
        {
            rotacao += 1;
            if (HasStateAuthority)
                RPC_ChangedVoid();
        }

    }
}
