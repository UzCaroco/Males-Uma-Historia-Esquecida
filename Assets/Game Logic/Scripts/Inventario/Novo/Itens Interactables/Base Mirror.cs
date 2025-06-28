using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BaseMirror : NetworkBehaviour, IInteractable
{
    NetworkBool direcao = true;
    [Networked] float rotacao { get; set; } = 0;
    [Networked] float rotacaoMinima { get; set; }
    [Networked] float rotacaoMaxima { get; set; }

    void ChangedVoid(float valor)
    {
        Debug.Log("Interagindo com o espelho");
        transform.parent.Rotate(Vector3.up * valor);
    }

    public override void Spawned()
    {
        rotacao = transform.parent.rotation.eulerAngles.y;
        rotacaoMinima = rotacao - 80;
        rotacaoMaxima = rotacao + 80;

        Debug.Log("Inicial" + rotacao);
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {

        if (direcao && rotacao < rotacaoMaxima)
        {
            rotacao += 1;
            ChangedVoid(1);
        }
        else if (direcao && rotacao >= rotacaoMaxima)
        {
            direcao = !direcao;

            rotacao -= 1;
            ChangedVoid(-1);
        }
        else if (!direcao && rotacao > rotacaoMinima)
        {
            rotacao -= 1;
            ChangedVoid(-1);
        }
        else if (!direcao && rotacao <= rotacaoMinima)
        {
            direcao = !direcao;

            rotacao += 1;
            ChangedVoid(1);
        }
    }
}
