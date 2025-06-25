using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCharada : MonoBehaviour
{
    [SerializeField] SlotCharada[] slots; // Array de slots para gerenciar os itens charada
    [SerializeField] sbyte[] sequencia; // Sequência correta dos IDs dos itens charada

    [SerializeField] GameObject Textos;

    bool charadaResolvida = false;
    void Update()
    {
        if (slots[0].itemCharada != null && slots[1].itemCharada != null &&
            slots[2].itemCharada != null && slots[3].itemCharada != null)
        {
            VerificarSequencia();
        }
    }

    void VerificarSequencia()
    {
        if (!charadaResolvida)
        {
            if (slots[0].itemCharada.idItem == sequencia[0] &&
            slots[1].itemCharada.idItem == sequencia[1] &&
            slots[2].itemCharada.idItem == sequencia[2] &&
            slots[3].itemCharada.idItem == sequencia[3])
            {
                Debug.Log("Charada resolvida!");
                charadaResolvida = true;
                Textos.SetActive(true); // Ativa o GameObject com os textos
            }
        }
    }
}
