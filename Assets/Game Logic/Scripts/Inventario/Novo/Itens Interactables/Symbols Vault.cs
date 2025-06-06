using Fusion;
using UnityEngine;

public class SymbolsVault : NetworkBehaviour, IInteractable
{
    [Networked] float codeVaultRotation { get; set; }
    [Networked] [SerializeField] int codeId { get; set; } = 0;
    [SerializeField] DoorVault doorVault;

    public override void Spawned()
    {
        
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ChangedVoid()
    {
        Debug.Log("Code state changed: " + codeVaultRotation);

        transform.Rotate(Vector3.left * 60);
        Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        doorVault.RPC_CodeChanged(codeId, codeVaultRotation);

    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        Debug.Log("Interagindo com codigo");

        if (codeVaultRotation < 300)
        {
            codeVaultRotation += 60;
            Debug.Log("Codigo do vault: " + codeVaultRotation);
        }
        else
        {
            codeVaultRotation = 0;
            Debug.Log("Codigo do vault: " + codeVaultRotation);
        }

        if (HasStateAuthority)
            RPC_ChangedVoid();

    }
}