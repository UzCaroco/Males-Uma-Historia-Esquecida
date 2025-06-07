using UnityEngine;

public class OulinesObjects : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    RaycastHit hit => Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f, layerMask) ? hitInfo : default;
    GameObject currentObject;
    private void Update()
    {
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Outline>(out Outline outline))
            {
                if (!outline.enabled)
                {
                    currentObject = hit.collider.gameObject;
                    outline.enabled = true;
                }
            }
        }
        else
        {
            if (currentObject != null)
            {
                if (currentObject.TryGetComponent<Outline>(out Outline outline))
                {
                    outline.enabled = false;
                }
                currentObject = null;
            }
        }
    }
}
