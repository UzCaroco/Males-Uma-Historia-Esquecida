using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PularVideo : MonoBehaviour
{
    [SerializeField] GameObject rawImage;

    public void Pular()
    {
        // Desativa o canvas do v�deo
        rawImage.SetActive(false);
    }
}
