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
    Vector3 left, right, top, down, topRight, topLeft, downLeft, downRight;


    NetworkBool follow = false;

    NetworkBool achouFoward => Physics.Raycast(origin, transform.forward, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouLeft => Physics.Raycast(origin, left, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouRight => Physics.Raycast(origin, right, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouTop => Physics.Raycast(origin, top, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouDown => Physics.Raycast(origin, down, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouTopRight => Physics.Raycast(origin, topRight, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouTopLeft => Physics.Raycast(origin, topLeft, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouDownLeft => Physics.Raycast(origin, downLeft, out RaycastHit hit, distanciaDeVisao);
    NetworkBool achouDownRight => Physics.Raycast(origin, downRight, out RaycastHit hit, distanciaDeVisao);


    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip somPassos;
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

        if (aud == null)
        {
            Debug.LogWarning("AUDIOSOURCE NAO ENCONTRADO AAAAAAAAAAAA: " + aud);
            aud = GetComponent<AudioSource>();
        }
    }

    bool VerificarRay(Vector3 origem, Vector3 direcao)
    {
        RaycastHit hit;

        // Mostra no editor
        Debug.DrawRay(origem, direcao * distanciaDeVisao, Color.red);

        // Faz o raycast
        if (Physics.Raycast(origem, direcao, out hit, distanciaDeVisao))
        {
            if (hit.collider.CompareTag("Player") && !follow) //Se visualizar o player e não estiver procurando por ele, passa a procurar e seguir
            {
                Debug.Log("Player detectado: " + hit.collider.name);
                follow = true; // Ativa a busca pelo player

                patrolScript.lookPlayer = true; // Ativa a busca pelo player
                patrolScript.playerEncontrado = hit.collider.transform; // Define o transform do player para poder segui-lo
            }
            return true; // Retorna verdadeiro se o raycast encontrar algo
        }
        return false;
    }

    
    public override void FixedUpdateNetwork()
    {
        NetworkBool encontrouPlayer = false; // Variável para verificar se o player foi encontrado
        // Se os raycast encontrar o player
        encontrouPlayer |= VerificarRay(origin, transform.forward);
        encontrouPlayer |= VerificarRay(origin, left);
        encontrouPlayer |= VerificarRay(origin, right);
        encontrouPlayer |= VerificarRay(origin, top);
        encontrouPlayer |= VerificarRay(origin, down);
        encontrouPlayer |= VerificarRay(origin, topRight);
        encontrouPlayer |= VerificarRay(origin, topLeft);
        encontrouPlayer |= VerificarRay(origin, downLeft);
        encontrouPlayer |= VerificarRay(origin, downRight);


        if (!encontrouPlayer && follow) // Se não encontrar o player e já estiver seguindo, desativa a busca
        {
            follow = false; // Desativa a busca pelo player
            patrolScript.lookPlayer = false; // Desativa a busca pelo player
            patrolScript.playerEncontrado = null; // Limpa o transform do player
        }
    }

    public void SomPassos()
    {
        aud.PlayOneShot(somPassos);
    }
}
