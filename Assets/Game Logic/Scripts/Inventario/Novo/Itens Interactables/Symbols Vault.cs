using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SymbolsVault : NetworkBehaviour, IInteractable
{
    [Networked, OnChangedRender(nameof(RotateServerRPC))] public Quaternion codeVaultTransform { get; set; }
    public void Interact(PlayerInventory playerInventory)
    {
        if (HasStateAuthority)
        {
            codeVaultTransform = Quaternion.Euler(Vector3.right * 60);
            //RotateServerRPC();
            Debug.Log("You have authority to rotate this object.");
        }
        else
        {
            Debug.Log("You don't have authority to rotate this object.");
        }
            
    }

    void RotateServerRPC()
    {
        Debug.Log("Rotating object on the server.");
        transform.Rotate(codeVaultTransform.x, codeVaultTransform.y, codeVaultTransform.z);
    }

    
}