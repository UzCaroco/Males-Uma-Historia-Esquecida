using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] RectTransform rectTransformImage;

    void Update()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;

        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
        float size = dist * 0.1f;
        rectTransformImage.sizeDelta = new Vector2(size, size);
    }
}
