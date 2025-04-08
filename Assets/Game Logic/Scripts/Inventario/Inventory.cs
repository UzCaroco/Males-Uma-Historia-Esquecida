using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Image itemIcon;

    public Item itemAtual = null;
    

    public bool TemItem() => itemAtual != null && itemAtual.type != ItemType.Default;


    private void Start()
    {
        Debug.Log($"name: {itemAtual.name}, Tipo: {itemAtual.type}, id: {itemAtual.id}");
    }

    public void PegarItem(Item novoItem)
    {
        if (TemItem())
        {
            Debug.Log("J� est� carregando um item!");
            return;
        }

        itemAtual = novoItem;
        itemIcon.sprite = itemAtual.icon;
        Debug.Log($"Pegou o item: {itemAtual.name}");
    }

    public void UsarItem()
    {
        if (!TemItem())
        {
            Debug.Log("N�o tem item para usar!");
            return;
        }

        // Aqui voc� pode adicionar a l�gica para usar o item
        Debug.Log($"Usou o item: {itemAtual.name}");
        itemIcon.sprite = null; // Remove o �cone do item
        itemAtual = null;
    }

    public void DevolverItem()
    {
        if (!TemItem())
        {
            Debug.Log("N�o tem item para devolver!");
            return;
        }
        // Aqui voc� pode adicionar a l�gica para devolver o item
        Debug.Log($"Devolveu o item: {itemAtual.name}");
        itemIcon.sprite = null; // Remove o �cone do item
        itemAtual = null;
    }


}
