using UnityEngine;

public class OulinesObjects : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    GameObject currentObject;
    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentObject)
            {
                // Desativa o anterior
                if (currentObject != null && currentObject.TryGetComponent<Outline>(out Outline currentOutline))
                {
                    currentOutline.enabled = false;
                }

                if (hitObject.TryGetComponent<Outline>(out Outline newOutline))
                {
                    // Ativa o novo
                    newOutline.enabled = true;
                    currentObject = hitObject;
                }
            }
            else
            {
                currentObject = null;
            }

        }
        else
        {
            if (currentObject != null && currentObject.TryGetComponent<Outline>(out Outline currentOutline))
            {
                // Desativa o objeto atual se não houver hit
                currentOutline.enabled = false;
                currentObject = null;
            }
        }
    }
}
