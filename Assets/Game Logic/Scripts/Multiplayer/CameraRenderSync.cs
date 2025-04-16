using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraRenderSync : MonoBehaviour
{
    Transform referenciaDaCamera;

    private void Start()
    {


        foreach (var playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (playerObject != null)
            {
                var networkObject = playerObject.GetComponent<NetworkObject>();
                if (networkObject != null && networkObject.IsLocalPlayer)
                {
                    referenciaDaCamera = playerObject.gameObject.GetComponentInChildren<Camera>().transform;
                    break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (referenciaDaCamera != null)
        {
            // Diferen�a entre a c�mera render e a do player
            Vector3 diferenca = transform.position - referenciaDaCamera.position;

            // Posi��o relativa apenas no plano XZ (horizontal)
            Vector3 dirXZ = new Vector3(diferenca.x, 0, diferenca.z).normalized;

            // �ngulo horizontal entre frente da c�mera e o alvo
            float angleY = Vector3.SignedAngle(Vector3.forward, dirXZ, Vector3.up);

            // Clamp no �ngulo
            angleY = Mathf.Clamp(angleY, -65f, 65f);

            // Aplica rota��o apenas em Y (horizontal)
            transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }

}
