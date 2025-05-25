using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMirror : MonoBehaviour, IInteractable
{
    bool direcao = true;
    float rotacao = 0;

    private void Start()
    {
        rotacao = transform.eulerAngles.y;
        Debug.Log("Inicial" + rotacao);
    }
    public void OnInteractObject(PlayerInventory playerInventory)
    {
        Debug.Log("Interagindo com o espelho");
        if (direcao && rotacao < 80)
        {
            transform.parent.Rotate(Vector3.up * 1);
            rotacao += 1;
            Debug.Log(rotacao);
        }
        else if (direcao && rotacao >= 80)
        {
            direcao = false;
            transform.parent.Rotate(Vector3.up * -1);
            rotacao -= 1;
        }
        else if (!direcao && rotacao > -80)
        {
            transform.parent.Rotate(Vector3.up * -1);
            rotacao -= 1;
            Debug.Log(rotacao);
        }
        else if (!direcao && rotacao <= -80)
        {
            direcao = true;
            transform.parent.Rotate(Vector3.up * 1);
            rotacao += 1;
        }

    }
}
