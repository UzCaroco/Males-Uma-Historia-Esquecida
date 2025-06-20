using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class FollowPlayer : NetworkBehaviour
{
    [SerializeField] Patrol patrolScript;


    [SerializeField] float raioDeVisao = 3f;
    [SerializeField] float distanciaDeVisao = 2f;

    [SerializeField] Vector3 offset = new Vector3(0, 1.5f, 0);
    Vector3 origin;
    [SerializeField] LayerMask layerMask;
    Vector3 left, right, top, down, topRight, topLeft, downLeft, downRight;


    NetworkBool follow = false;

    NetworkBool achouFoward => Physics.Raycast(origin, transform.forward, distanciaDeVisao, layerMask);
    NetworkBool achouLeft => Physics.Raycast(origin, left, distanciaDeVisao, layerMask);
    NetworkBool achouRight => Physics.Raycast(origin, right, distanciaDeVisao, layerMask);
    NetworkBool achouTop => Physics.Raycast(origin, top, distanciaDeVisao, layerMask);
    NetworkBool achouDown => Physics.Raycast(origin, down, distanciaDeVisao, layerMask);
    NetworkBool achouTopRight => Physics.Raycast(origin, topRight, distanciaDeVisao, layerMask);
    NetworkBool achouTopLeft => Physics.Raycast(origin, topLeft, distanciaDeVisao, layerMask);
    NetworkBool achouDownLeft => Physics.Raycast(origin, downLeft, distanciaDeVisao, layerMask);
    NetworkBool achouDownRight => Physics.Raycast(origin, downRight, distanciaDeVisao, layerMask);


    private void Update()
    {
        origin = transform.position + offset;

        left = transform.forward + transform.TransformDirection(-raioDeVisao, 0, 0);
        right = transform.forward + transform.TransformDirection(raioDeVisao, 0, 0);
        top = transform.forward + transform.TransformDirection(0, raioDeVisao, 0);
        down = transform.forward + transform.TransformDirection(0, -raioDeVisao, 0);
        topRight = transform.forward + transform.TransformDirection(raioDeVisao, raioDeVisao, 0);
        topLeft = transform.forward + transform.TransformDirection(-raioDeVisao, raioDeVisao, 0);
        downLeft = transform.forward + transform.TransformDirection(-raioDeVisao, -raioDeVisao, 0);
        downRight = transform.forward + transform.TransformDirection(raioDeVisao, -raioDeVisao, 0);

        
    }

    void VerificarRay(Vector3 origem, Vector3 direcao)
    {
        RaycastHit hit;

        // Mostra no editor
        Debug.DrawRay(origem, direcao * distanciaDeVisao, Color.red);

        // Faz o raycast
        if (Physics.Raycast(origem, direcao, out hit, distanciaDeVisao, layerMask))
        {
            GameObject alvo = hit.collider.gameObject;

            if (alvo.CompareTag("Player") && !follow) //Se visualizar o player e não estiver procurando por ele, passa a procurar e seguir
            {
                Debug.Log("Player detectado: " + alvo.name);
                follow = true; // Ativa a busca pelo player

                patrolScript.lookPlayer = true; // Ativa a busca pelo player
                patrolScript.playerEncontrado = alvo.transform; // Define o transform do player para poder segui-lo
            }
        }
    }

    
    public override void FixedUpdateNetwork()
    {
        if (achouFoward || achouLeft || achouRight || achouTop || achouDown || achouTopRight || achouTopLeft || achouDownLeft || achouDownRight)
        {
            // Se o raycast encontrar algo, Verifica cada direção
            VerificarRay(origin, transform.forward);
            VerificarRay(origin, left);
            VerificarRay(origin, right);
            VerificarRay(origin, top);
            VerificarRay(origin, down);
            VerificarRay(origin, topRight);
            VerificarRay(origin, topLeft);
            VerificarRay(origin, downLeft);
            VerificarRay(origin, downRight);
        }
        else
        {
            Debug.Log("Player NÃO detectado: ");
            follow = false; // Se o raycast não atingir mais o player, desativa a busca
            patrolScript.lookPlayer = false; // Desativa a busca pelo player
        }
    }
}
