using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrocarEnigma : MonoBehaviour
{
    public List<string> charadas = new List<string>();

    [SerializeField] TextMeshProUGUI texto;
    int index = 0;

    public void Trocar()
    {
        if (index < charadas.Count)
        {
            texto.text = charadas[index];
            index++;
        }
        else
        {
            index = 0;
            texto.text = charadas[index];
        }
    }

    public void NovaCharada(string charada)
    {
        charadas.Add(charada);

        if (charadas.Count == 1)
        {
            texto.text = charada; // Se for a primeira charada, exibe-a imediatamente
        }
    }
}
