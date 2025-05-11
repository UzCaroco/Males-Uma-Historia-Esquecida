using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsVault : MonoBehaviour, IInteractable
{

    public void Interact(PlayerInventory playerInventory)
    {
        Debug.Log("Interagindo com o Codigo");

        transform.Rotate(Vector3.right * 60);

    }
}