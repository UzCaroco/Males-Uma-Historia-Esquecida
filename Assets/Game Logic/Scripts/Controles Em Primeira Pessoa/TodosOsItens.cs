using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TodosOsItens : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask interactableLayer;

    public RaycastHit hitInteract => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    public Transform dropPoint;
    public ItemData itemAtual = null;

    public float rayDistance = 100f;

    FirstPersonCamera cameraPessoal;






    [Networked] int chestId { get; set; } = 0;
    [Networked] Vector3 chestState { get; set; }
    NetworkBool open = false;

    void Start()
    {
        Debug.Log($"Start - Tem Input Authority? {Object.HasInputAuthority}");
        Debug.Log($"Start - Tem State Authority? {Object.HasStateAuthority}");
    }

    public override void Spawned()
    {
        cam = Camera.main;
        cameraPessoal = cam.GetComponent<FirstPersonCamera>();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ChangedVoid()
    {
        Debug.Log("Chest state changed: " + chestState);

        if (!open)
        {
            Debug.Log(chestState);

            transform.Rotate(chestState);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        }
        else
        {
            Debug.Log(chestState);

            transform.Rotate(chestState);
            Debug.Log(transform.rotation.eulerAngles + "ROTACAO DO OBJETO");

        }


    }










    public void OnInteractObject(Inven playerInventory)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        Debug.Log("Interagindo com o porta do baú");

        if (!open)
        {
            chestState = new Vector3(-90, 0, 0);
            open = true;

            RPC_ChangedVoid();

            chestId++;
            Debug.Log("ID DO BAU " + chestId);
        }
        else
        {
            chestState = new Vector3(90, 0, 0);
            open = false;

            RPC_ChangedVoid();

            chestId++;
            Debug.Log("ID DO BAU " + chestId);
        }

    }



    public override void FixedUpdateNetwork()
    {
        cameraPessoal.UpdateInteragir();
    }
}
