using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class DoorVault : MonoBehaviour, IInteractable
{
    bool codigoCorreto = true;



    public void OnInteractObject(Inven playerInventory)
    {
        Debug.Log("Interagindo com o porta do cofre");

        transform.Rotate(Vector3.up * 90);

    }
}
