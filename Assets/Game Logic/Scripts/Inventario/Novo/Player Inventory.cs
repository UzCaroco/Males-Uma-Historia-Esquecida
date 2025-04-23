using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask interactableLayer;

    public RaycastHit hitInteract => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    public Transform dropPoint;
    public Image itemIcon;
    public ItemData itemAtual = null;

    public void Interagir()
    {
        if (hitInteract.collider)
        {
            hitInteract.collider.TryGetComponent(out IInteractable interactable);

            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
        else
        {
            Debug.Log("Não tem nada pra interagir.");
        }
    }

}
