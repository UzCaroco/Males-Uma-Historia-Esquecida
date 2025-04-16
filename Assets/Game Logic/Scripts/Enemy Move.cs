using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] sbyte velocidade = 5; // Velocidade do inimigo
    public Vector3 vetorMovimento;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Quaternion rotacao = Quaternion.LookRotation(vetorMovimento); // Cria uma rotação para o inimigo
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, 10 * Time.deltaTime); // Rotaciona o inimigo para a direção do movimento

        if (transform.rotation == rotacao)
        {
            Vector3 andar = transform.forward * velocidade * Time.deltaTime; // Cria um vetor de movimento
            controller.Move(andar); // Move o inimigo
        }
    }
}
