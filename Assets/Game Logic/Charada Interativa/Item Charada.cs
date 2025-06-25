using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCharada : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    Vector2 initialPosition;

    [SerializeField] List<RectTransform> slots; // 4 slots poss�veis

    bool dentroDoAlvo = false;
    public sbyte idItem; // ID do item charada, usado para identificar o item

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RectTransform slotValido = VerificarSlotValido();

        if (slotValido != null)
        {
            if (slotValido.TryGetComponent<SlotCharada>(out SlotCharada slotCharada))
            {
                if (slotCharada.itemCharada == null)
                {
                    slotCharada.itemCharada = this; // Atribui este item ao slot
                    rectTransform.anchoredPosition = slotValido.anchoredPosition; // Encaixa no slot
                }
                else
                {
                    slotCharada.itemCharada.rectTransform.anchoredPosition = slotCharada.itemCharada.initialPosition; // Volta o item anterior para o in�cio
                    slotCharada.itemCharada = this; // Atribui este item ao slot
                    rectTransform.anchoredPosition = slotValido.anchoredPosition; // Encaixa no slot
                }
            }
            else
            {
                Debug.LogWarning("O slot n�o possui o componente SlotCharada.");
            }
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition; // Volta pro in�cio
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        initialPosition = rectTransform.anchoredPosition;
    }

    RectTransform VerificarSlotValido()
    {
        foreach (var slot in slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(slot, Input.mousePosition, canvas.worldCamera))
            {
                return slot;
            }
        }

        return null; // Nenhum slot encontrado
    }

}
