using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MirrorItem : NetworkBehaviour, IInteractable
{
    NetworkTRSP NetworkTRSP;
    NetworkBool direcao = true;
    [Networked, OnChangedRender(nameof(ChangedVoid))] float rotacao { get; set; } = 0;

    
    void ChangedVoid()
    {
        Debug.Log("Interagindo com o espelho");
        if (direcao && rotacao < 80)
        {
            NetworkTRSP.transform.Rotate(Vector3.right * 1);
            Debug.Log(rotacao);
        }
        else if (direcao && rotacao >= 80)
        {
            direcao = false;
            NetworkTRSP.transform.Rotate(Vector3.left * -1);
        }
        else if (!direcao && rotacao > -80)
        {
            NetworkTRSP.transform.Rotate(Vector3.right * -1);
            Debug.Log(rotacao);
        }
        else if (!direcao && rotacao <= -80)
        {
            direcao = true;
            NetworkTRSP.transform.Rotate(Vector3.left * 1);
        }

    }


    private void Start()
    {
        NetworkTRSP = GetComponent<NetworkTRSP>();
        rotacao = transform.eulerAngles.x;
        Debug.Log("Inicial" + rotacao);
    }
    public void OnInteractObject(PlayerInventory playerInventory)
    {
        if (direcao && rotacao < 80)
        {
            rotacao += 1;
        }
        else if (direcao && rotacao >= 80)
        {
            rotacao -= 1;
        }
        else if (!direcao && rotacao > -80)
        {
            rotacao -= 1;
        }
        else if (!direcao && rotacao <= -80)
        {
            rotacao += 1;
        }

    }
}
