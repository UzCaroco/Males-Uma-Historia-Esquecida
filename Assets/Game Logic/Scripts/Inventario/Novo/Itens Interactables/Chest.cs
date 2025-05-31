using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Chest : NetworkBehaviour, IInteractable
{
    [Networked, OnChangedRender(nameof(ChangedVoid))] Vector3 chestState { get; set; }

    void ChangedVoid()
    {
        Debug.Log("Chest state changed: " + chestState);

        if (!open)
        {
            Debug.Log(chestState);

            NetworkTRSP.transform.Rotate(chestState);
            
        }
        else
        {
            Debug.Log(chestState);
            NetworkTRSP.transform.Rotate(chestState);
            
        }
    }






    NetworkTRSP NetworkTRSP;
    NetworkBool open = false;

    private void Start()
    {
        NetworkTRSP = GetComponent<NetworkTRSP>();
        if (NetworkTRSP == null)
        {
            Debug.LogError("NetworkTRSP component not found on the object.");
        }
    }


    
    public void OnInteractObject(PlayerInventory playerInventory)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        Debug.Log("Interagindo com o porta do baú");

        if (!open)
        {
            chestState = new Vector3(90, 0, 0);
            open = true;
        }
        else
        {
            chestState = new Vector3(-90, 0, 0);
            open = false;
        }

    }
}
