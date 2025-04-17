using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] sbyte velocidade = 5; // Velocidade do inimigo
    public Vector3 vetorMovimento;
    public bool podeAndar = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Quaternion rotacao = Quaternion.LookRotation(vetorMovimento); // Cria uma rotação para o inimigo
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, 20 * Time.deltaTime); // Rotaciona o inimigo para a direção do movimento

        if (podeAndar)
        {
            Vector3 andar = transform.forward * velocidade * Time.deltaTime + rb.position; // Cria um vetor de movimento
            rb.MovePosition(andar); // Move o inimigo
        }
    }
}
