using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask layerGrab, layerUse, layerReturn;

    public RaycastHit lookToGrab => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, layerGrab) ? hit : default;
    public RaycastHit lookToUse => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, layerUse) ? hit : default;
    public RaycastHit lookToReturn => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, layerReturn) ? hit : default;


    public ItemData itemAtual = null;


    public bool TemItem() => itemAtual != null;

    public void Interagir()
    {
        if (lookToGrab.collider)
        {
            PegarItem();
        }
        else if (lookToUse.collider)
        {
            UsarItem();
        }
        else if (lookToReturn.collider)
        {
            DevolverItem();
        }
        else
        {
            Debug.Log("Não tem nada pra interagir.");
        }
    }

    private void PegarItem()
    {
        try
        {
            if (TemItem() && lookToGrab.collider)
            {
                Debug.Log("Já tem um item na mão!");
                return;
            }

            ItemLogic itemLogic = lookToGrab.collider.GetComponent<ItemLogic>();
            itemAtual = itemLogic.dadosDoItem;
            Debug.Log($"Pegou o item: {itemAtual.name}");

            itemIcon.sprite = itemAtual.icon; // Atualiza o ícone do item
            itemAtual.position = lookToGrab.collider.transform.position; // Armazena a posição do item

            Destroy(lookToGrab.collider.gameObject); // Destroi o objeto do mundo
        }
        catch (System.Exception)
        {
        }
    }

    private void UsarItem()
    {
        try
        {
            if (!TemItem() && lookToUse.collider)
            {
                Debug.Log("Não tem item para usar!");
                return;
            }

            Debug.Log($"Usou o item: {itemAtual.name}");
            itemIcon.sprite = null; // Remove o ícone do item
            itemAtual = null;

            Destroy(lookToUse.collider.gameObject); // Destroi o objeto do mundo
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void DevolverItem()
    {
        try
        {
            if (!TemItem() && lookToReturn.collider)
            {
                Debug.Log("Não tem item para devolver!");
                return;
            }

            Instantiate(itemAtual.itemPrefab, itemAtual.position, Quaternion.identity); // Cria o objeto no mundo

            Debug.Log($"Devolveu o item: {itemAtual.name}");
            itemIcon.sprite = null; // Remove o ícone do item
            itemAtual = null;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }
}
