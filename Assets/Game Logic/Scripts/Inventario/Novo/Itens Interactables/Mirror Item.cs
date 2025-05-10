using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorItem : MonoBehaviour, IInteractable
{
    bool direcao = true;
    float rotacao = 0;

    private void Start()
    {
        rotacao = transform.eulerAngles.x;
        Debug.Log("Inicial" + rotacao);
    }
    public void Interact(PlayerInventory playerInventory)
    {
        Debug.Log("Interagindo com o espelho");
        if (direcao && rotacao < 80)
        {
            transform.Rotate(Vector3.right * 5);
            rotacao += 5;
            Debug.Log(rotacao);
        }
        else if (direcao && rotacao >= 80)
        {
            direcao = false;
            transform.Rotate(Vector3.left * -5);
            rotacao -= 5;
        }
        else if (!direcao && rotacao > -80)
        {
            transform.Rotate(Vector3.right * -5);
            rotacao -= 5;
            Debug.Log(rotacao);
        }
        else if (!direcao && rotacao <= -80)
        {
            direcao = true;
            transform.Rotate(Vector3.left * 5);
            rotacao += 5;
        }

    }
}
