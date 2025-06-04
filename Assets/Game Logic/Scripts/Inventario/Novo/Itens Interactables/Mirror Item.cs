using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MirrorItem : NetworkBehaviour, IInteractable
{
    NetworkTRSP NetworkTRSP;
    NetworkBool direcao = true;
    [Networked, OnChangedRender(nameof(ChangedVoid))] float rotacao { get; set; } = 0;

    [Networked] float min { get; set; }
    [Networked] float max { get; set; }
    void ChangedVoid()
    {
        Debug.Log("Interagindo com o espelho");
        if (direcao && rotacao < max)
        {
            NetworkTRSP.transform.Rotate(Vector3.right * 0.5f);
            Debug.Log(rotacao);
        }
        else if (direcao && rotacao >= max)
        {
            direcao = false;
            NetworkTRSP.transform.Rotate(Vector3.left * -0.5f);
        }
        else if (!direcao && rotacao > min)
        {
            NetworkTRSP.transform.Rotate(Vector3.right * -0.5f);
            Debug.Log(rotacao);
        }
        else if (!direcao && rotacao <= min)
        {
            direcao = true;
            NetworkTRSP.transform.Rotate(Vector3.left * 0.5f);
        }

    }


    private void Start()
    {
        NetworkTRSP = GetComponent<NetworkTRSP>();
        rotacao = NetworkTRSP.transform.rotation.x;
        Debug.Log(rotacao);
        min = rotacao - 80;
        max = rotacao + 80;
        Debug.Log(min + " " + max);
        Debug.Log(this.gameObject.transform.parent.name);
    }
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (direcao && rotacao < max)
        {
            rotacao += 1;
        }
        else if (direcao && rotacao >= max)
        {
            rotacao -= 1;
        }
        else if (!direcao && rotacao > min)
        {
            rotacao -= 1;
        }
        else if (!direcao && rotacao <= min)
        {
            rotacao += 1;
        }

    }
}
