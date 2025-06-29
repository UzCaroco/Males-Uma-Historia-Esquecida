using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ClosetDoor : NetworkBehaviour, IInteractable
{
    [SerializeField] NetworkObject portaDireita, portaEsquerda;

    [Networked] NetworkBool isOpen { get; set; } = false;

    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (!isOpen)
        {
            RPC_OpenDoor();
        }
        else
        {
            RPC_CloseDoor();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        GetComponent<AudioSource>().Play();

        portaDireita.transform.localRotation = Quaternion.Euler(0, 90, 0);
        portaEsquerda.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;

        GetComponent<AudioSource>().Play();

        portaDireita.transform.localRotation = Quaternion.Euler(0, 0, 0);
        portaEsquerda.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
