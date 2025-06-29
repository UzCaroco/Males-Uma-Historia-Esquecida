using Fusion;
using UnityEngine;

public class InteriorDoor : NetworkBehaviour, IInteractable
{
    [Networked] int doorId { get; set; } = 0;
    [Networked] float doorState { get; set; }
    NetworkBool open = false;

    [SerializeField] bool portaDaCamara = false;
    public NetworkBool destravado = false;

    AudioSource audioSource;
    [SerializeField] AudioClip abrirPorta, fecharPorta;

    public override void Spawned()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ChangedVoid()
    {
        Debug.Log("Chest state changed: " + doorState);

        if (!open)
        {
            Debug.Log(doorState);

            audioSource.PlayOneShot(abrirPorta);

            transform.Rotate(Vector3.forward * doorState);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        }
        else
        {
            Debug.Log(doorState);

            audioSource.PlayOneShot(fecharPorta);

            transform.Rotate(Vector3.forward * doorState);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        Debug.Log("Interagindo com o porta do baú");

        if (!portaDaCamara)
        {
            if (!open)
            {
                doorState = -90;
                open = true;

                if (HasStateAuthority)
                    RPC_ChangedVoid();

                doorId++;
                Debug.Log("ID DA PORTA " + doorId);
            }
            else
            {
                doorState = 90;
                open = false;

                if (HasStateAuthority)
                    RPC_ChangedVoid();

                doorId++;
                Debug.Log("ID DA PORTA " + doorId);
            }
        }
        else
        {
            if (destravado)
            {
                if (!open)
                {
                    doorState = -90;
                    open = true;

                    if (HasStateAuthority)
                        RPC_ChangedVoid();

                    doorId++;
                    Debug.Log("ID DA PORTA " + doorId);
                }
                else
                {
                    doorState = 90;
                    open = false;

                    if (HasStateAuthority)
                        RPC_ChangedVoid();

                    doorId++;
                    Debug.Log("ID DA PORTA " + doorId);
                }
            }
        }

    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_EnemyOpenDoor()
    {
        Debug.Log("Inimigo entrou no gatilho da porta");

        if (!open)
        {
            doorState = -90;
            open = true;

            RPC_ChangedVoid();
            doorId++;
            Debug.Log("ID DA PORTA " + doorId);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_EnemyCloseDoor()
    {
        Debug.Log("Inimigo saiu no gatilho da porta");

        if (open)
        {
            doorState = 90;
            open = false;

            RPC_ChangedVoid();
            doorId++;
            Debug.Log("ID DA PORTA " + doorId);
        }
    }
}
