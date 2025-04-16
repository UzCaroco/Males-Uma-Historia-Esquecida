using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] LayerMask layerParede;
    RaycastHit frente => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 4, layerParede) ? hit : default; // Raycast para frente do objeto
    RaycastHit direita => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit hit, 4, layerParede) ? hit : default; // Raycast para direita do objeto
    RaycastHit esquerda => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out RaycastHit hit, 4, layerParede) ? hit : default; // Raycast para esquerda do objeto
    RaycastHit tras => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out RaycastHit hit, 4, layerParede) ? hit : default; // Raycast para tras do objeto

    RaycastHit[] raycasts => new RaycastHit[] { frente, tras, direita, esquerda }; // Array de raycasts

    EnemyMove enemyMove; // Referência ao script EnemyMove

    void Start()
    {
        enemyMove = GetComponent<EnemyMove>(); // Pega o componente EnemyMove
        StartCoroutine(Espera()); // Chama a coroutine novamente
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 4, Color.red); // Desenha um raio vermelho para frente do objeto
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 4, Color.green); // Desenha um raio verde para direita do objeto
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 4, Color.blue); // Desenha um raio azul para esquerda do objeto
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 4, Color.yellow); // Desenha um raio amarelo para tras do objeto
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ponto De Sorteio"))
        {
            List<(RaycastHit opcao, sbyte peso, Vector3 direcao)> opcoes = new List<(RaycastHit opcao, sbyte peso, Vector3 direcao)>(); // Lista de opções para o sorteio

            for (int i = 0; i < raycasts.Length; i++)
            {
                if (raycasts[i].collider == null)
                {
                    if (i == 0)
                    {
                        opcoes.Add((raycasts[i], 1, new Vector3(0, 0, 1))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para frente
                    }
                    else if (i == 1)
                    {
                        opcoes.Add((raycasts[i], 1, new Vector3(0, 0, -1))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para trás
                    }
                    else if (i == 2)
                    {
                        opcoes.Add((raycasts[i], 1, new Vector3(1, 0, 0))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para direita
                    }
                    else if (i == 1)
                    {
                        opcoes.Add((raycasts[i], 1, new Vector3(-1, 0, 0))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para esquerda
                    }

                    Debug.Log("Raycast " + i + " não colidiu com nada"); // Loga que o raycast não colidiu com nada
                }
                else if (raycasts[i].collider != null)
                {
                    Debug.Log("Raycast " + i + " colidiu com a parede"); // Loga que o raycast colidiu com a parede
                }
            }

            Vector3 direcaoSorteada = SorteioComPorcentagem(opcoes); // Chama o método de sorteio com porcentagem passando as opções



        }
    }

    Vector3 SorteioComPorcentagem(List<(RaycastHit opcao, sbyte peso, Vector3 direcao)> opcoes)
    {
        Debug.Log("Sorteio com porcentagem: " + opcoes.Count + " opções disponíveis"); // Loga a quantidade de opções disponíveis

        sbyte somaPesos = (sbyte)opcoes.Sum(x => x.peso); // Soma os pesos de todas as opções
        Debug.Log("Soma dos pesos: " + somaPesos); // Loga a soma dos pesos
        sbyte acumulado = 0;
        sbyte sorteio = (sbyte)Random.Range(0, somaPesos); // Sorteia um número entre 0 e 99
        Debug.Log("Sorteio: " + sorteio); // Loga o número sorteado

        foreach (var x in opcoes)
        {
            acumulado += x.peso;

            if (sorteio < acumulado)
            {
                Debug.Log("Direção sorteada: " + x.direcao); // Loga a direção sorteada
                return x.direcao; // Retorna a opção correspondente

            }
        }

        return default; // Retorna default se não encontrar nenhuma opção
    }

    IEnumerator Espera()
    {
        List<(RaycastHit opcao, sbyte peso, Vector3 direcao)> opcoes = new List<(RaycastHit opcao, sbyte peso, Vector3 direcao)>(); // Lista de opções para o sorteio

        for (int i = 0; i < raycasts.Length; i++)
        {
            if (raycasts[i].collider == null)
            {
                if (i == 0)
                {
                    opcoes.Add((raycasts[i], 1, new Vector3(0, 0, 1))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para frente
                }
                else if (i == 1)
                {
                    opcoes.Add((raycasts[i], 1, new Vector3(0, 0, -1))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para trás
                }
                else if (i == 2)
                {
                    opcoes.Add((raycasts[i], 1, new Vector3(1, 0, 0))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para direita
                }
                else if (i == 3)
                {
                    opcoes.Add((raycasts[i], 1, new Vector3(-1, 0, 0))); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para esquerda
                }

                Debug.Log("Raycast " + i + " não colidiu com nada"); // Loga que o raycast não colidiu com nada
            }
            else if (raycasts[i].collider != null)
            {
                Debug.Log("Raycast " + i + " colidiu com a parede"); // Loga que o raycast colidiu com a parede
            }
        }
        Vector3 direcaoSorteada = SorteioComPorcentagem(opcoes); // Chama o método de sorteio com porcentagem passando as opções
        enemyMove.vetorMovimento = direcaoSorteada; // Atualiza a direção do movimento do inimigo

        yield return new WaitForSeconds(3f); // Espera o tempo especificado
        StartCoroutine(Espera()); // Chama a coroutine novamente
    }
}
