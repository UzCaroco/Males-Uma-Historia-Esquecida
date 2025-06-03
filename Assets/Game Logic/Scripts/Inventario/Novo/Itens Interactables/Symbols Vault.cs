using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SymbolsVault : MonoBehaviour, IInteractable
{
    //[Networked, OnChangedRender(nameof(RotateServerRPC))] 
    public Quaternion codeVaultTransform { get; set; }
    public void OnInteractObject(Inven playerInventory)
    {
        Debug.Log("Interagindo com o símbolo");
        codeVaultTransform = Quaternion.Euler(Vector3.right * 60);

        transform.Rotate(Vector3.right * 60);

        /*if (HasStateAuthority)
        {
            codeVaultTransform = Quaternion.Euler(Vector3.right * 60);
            //RotateServerRPC();
            Debug.Log("You have authority to rotate this object.");
        }
        else
        {
            Debug.Log("You don't have authority to rotate this object.");
        }*/

    }

    void RotateServerRPC()
    {
        Debug.Log("Rotating object on the server.");
        transform.Rotate(codeVaultTransform.x, codeVaultTransform.y, codeVaultTransform.z);
    }

    
}