using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowPlayer : NetworkBehaviour
{
    [SerializeField] float raioDeVisao = 3f;
    [SerializeField] float distanciaDeVisao = 2f;

    [SerializeField] Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] LayerMask layerMask;
    Vector3 left, right, top, down;

    bool achou => Physics.Raycast(transform.position + offset, transform.forward, distanciaDeVisao, layerMask);
    bool achouLeft => Physics.Raycast(transform.position + offset, left, distanciaDeVisao, layerMask);
    bool achouRight => Physics.Raycast(transform.position + offset, right, distanciaDeVisao, layerMask);
    bool achouTop => Physics.Raycast(transform.position + offset, top, distanciaDeVisao, layerMask);
    bool achouDown => Physics.Raycast(transform.position + offset, down, distanciaDeVisao, layerMask);

    private void Update()
    {
        left = transform.forward + transform.TransformDirection(-raioDeVisao, 0, 0);
        right = transform.forward + transform.TransformDirection(raioDeVisao, 0, 0);
        top = transform.forward + transform.TransformDirection(0, raioDeVisao, 0);
        down = transform.forward - transform.TransformDirection(0, -raioDeVisao, 0);

        Debug.DrawRay(transform.position + offset, transform.forward * distanciaDeVisao, Color.red);
        Debug.DrawRay(transform.position + offset, left * distanciaDeVisao, Color.red);
        Debug.DrawRay(transform.position + offset, right * distanciaDeVisao, Color.red);
        Debug.DrawRay(transform.position + offset, top * distanciaDeVisao, Color.red);
        Debug.DrawRay(transform.position + offset, down * distanciaDeVisao, Color.red);
    }
    public override void FixedUpdateNetwork()
    {
        if (achou || achouLeft || achouRight || achouTop || achouDown)
        {
            
        }
    }
}
