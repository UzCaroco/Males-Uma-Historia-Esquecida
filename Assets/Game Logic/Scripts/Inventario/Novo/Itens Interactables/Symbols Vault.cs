using System.Collections;
using Fusion;
using UnityEngine;

public class SymbolsVault : NetworkBehaviour, IInteractable
{
    [Networked] float codeVaultRotation { get; set; }
    [Networked] [SerializeField] int codeId { get; set; } = 0;
    [SerializeField] DoorVault doorVault;

    [Networked] bool countdown { get; set; } = false;

    public override void Spawned()
    {
        
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ChangedVoid()
    {
        Debug.Log("Code state changed: " + codeVaultRotation);

        transform.Rotate(Vector3.left * 45);
        Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        doorVault.RPC_CodeChanged(codeId, codeVaultRotation);

    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (!countdown)
        {
            countdown = true;
            Debug.Log("Interagindo com codigo");

            if (codeVaultRotation < 345)
            {
                codeVaultRotation += 45;
                Debug.Log("Codigo do vault: " + codeVaultRotation);
            }
            else
            {
                codeVaultRotation = 0;
                Debug.Log("Codigo do vault: " + codeVaultRotation);
            }

            if (HasStateAuthority)
                RPC_ChangedVoid();

            StartCoroutine(CoultdownRotate());
        }
    }

    IEnumerator CoultdownRotate()
    {
        yield return new WaitForSeconds(2f);
        countdown = false;
    }
}