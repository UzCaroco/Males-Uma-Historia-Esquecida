using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] LayerMask layerParede, layerPontoDeSorteio;
    [SerializeField] Vector3 posicaoInicialDoRaycast;
    [SerializeField] sbyte posicaoEmYDoRaycast;

    RaycastHit frente => Physics.Raycast(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, layerParede) ? hit : default; // Raycast para frente do objeto
    RaycastHit direita => Physics.Raycast(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.right), out RaycastHit hit, 5, layerParede) ? hit : default; // Raycast para direita do objeto
    RaycastHit esquerda => Physics.Raycast(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.left), out RaycastHit hit, 5, layerParede) ? hit : default; // Raycast para esquerda do objeto
    RaycastHit tras => Physics.Raycast(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.back), out RaycastHit hit, 5, layerParede) ? hit : default; // Raycast para tras do objeto

    RaycastHit frentePonto => Physics.Raycast(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 50, layerPontoDeSorteio, QueryTriggerInteraction.Collide) ? hit : default; // Raycast para frente do objeto

    RaycastHit[] raycasts => new RaycastHit[] { frente, tras, direita, esquerda }; // Array de raycasts

    EnemyMove enemyMove; // Referência ao script EnemyMove

    Queue<Vector3> moves = new Queue<Vector3>(); // Fila de movimentos do inimigo
    Vector3 direcaoSorteada;

    void Start()
    {
        enemyMove = GetComponent<EnemyMove>(); // Pega o componente EnemyMove
    }

    void Update()
    {
        posicaoInicialDoRaycast = new Vector3(transform.position.x, posicaoEmYDoRaycast, transform.position.z); // Atualiza a posição inicial do raycast

        Debug.DrawRay(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.forward) * 50, Color.red); // Desenha um raio vermelho para frente do objeto
        Debug.DrawRay(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.right) * 5, Color.green); // Desenha um raio verde para direita do objeto
        Debug.DrawRay(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.left) * 5, Color.blue); // Desenha um raio azul para esquerda do objeto
        Debug.DrawRay(posicaoInicialDoRaycast, transform.TransformDirection(Vector3.back) * 5, Color.yellow); // Desenha um raio amarelo para tras do objeto
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ponto De Sorteio"))
        {
            Debug.Log(frentePonto.collider);
            Vector3 posicionar = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            transform.position = Vector3.Slerp(transform.position, posicionar, 1f);

            

            List<(RaycastHit opcao, sbyte peso, Vector3 direcao)> opcoes = new List<(RaycastHit opcao, sbyte peso, Vector3 direcao)>(); // Lista de opções para o sorteio

            for (int i = 0; i < raycasts.Length; i++)
            {
                if (raycasts[i].collider == null)
                {
                    if (i == 0)
                    {
                        opcoes.Add((raycasts[i], 1, transform.forward)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para frente
                    }
                    else if (i == 1)
                    {
                        opcoes.Add((raycasts[i], 1, -transform.forward)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para trás
                    }
                    else if (i == 2)
                    {
                        opcoes.Add((raycasts[i], 1, transform.right)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para direita
                    }
                    else if (i == 3)
                    {
                        opcoes.Add((raycasts[i], 1, -transform.right)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para esquerda
                    }

                    Debug.Log("Raycast " + i + " não colidiu com nada"); // Loga que o raycast não colidiu com nada
                }
                else if (raycasts[i].collider != null)
                {
                    Debug.Log("Raycast " + i + " colidiu com a parede"); // Loga que o raycast colidiu com a parede
                }
            }
            if (frentePonto.collider != null)
            {
                if (moves.Count > 2)
                {
                    StartCoroutine(SorteioComPorcentagem(opcoes));
                }

                if (moves.Count < 3)
                {
                    moves.Enqueue(other.transform.position); // Adiciona a posição do ponto de sorteio na fila
                }
                else
                {
                    moves.Dequeue(); // Remove a posição mais antiga da fila
                    moves.Enqueue(other.transform.position); // Adiciona a nova posição na fila
                }
                
            }

            
        }
    }

    IEnumerator SorteioComPorcentagem(List<(RaycastHit opcao, sbyte peso, Vector3 direcao)> opcoes)
    {
        sbyte somaPesos = (sbyte)opcoes.Sum(x => x.peso); // Soma os pesos de todas as opções
        sbyte acumulado = 0;
        sbyte sorteio = (sbyte)Random.Range(0, somaPesos); // Sorteia um número entre 0 e 99

        foreach (var op in opcoes)
        {
            acumulado += op.peso;

            if (sorteio < acumulado)
            {
                Debug.Log("Direção sorteada: " + op.direcao); // Loga a direção sorteada

                enemyMove.podeAndar = false;
                enemyMove.vetorMovimento = op.direcao; // Define a direção do movimento do inimigo

                yield return new WaitForSeconds(0.8f); // Espera o tempo especificado

                //Quando rotacionar, vai verificar se aquele ponto está entre os três ultimos pontos
                if (moves.Any(x => Vector3.Distance(x, frentePonto.collider.transform.position) < 0.1f))
                {
                    List<(RaycastHit opcao, sbyte peso, Vector3 direcao)> opcoesSecundarias = new List<(RaycastHit opcao, sbyte peso, Vector3 direcao)>(); // Lista de opções para o sorteio

                    for (int i = 0; i < raycasts.Length; i++)
                    {
                        if (raycasts[i].collider == null)
                        {
                            if (i == 0)
                            {
                                opcoesSecundarias.Add((raycasts[i], 1, transform.forward)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para frente
                            }
                            else if (i == 1)
                            {
                                opcoesSecundarias.Add((raycasts[i], 1, -transform.forward)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para trás
                            }
                            else if (i == 2)
                            {
                                opcoesSecundarias.Add((raycasts[i], 1, transform.right)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para direita
                            }
                            else if (i == 3)
                            {
                                opcoesSecundarias.Add((raycasts[i], 1, -transform.right)); // Adiciona a opção com peso 1 se o raycast não colidiu com nada com vetor para esquerda
                            }

                            Debug.Log("Raycast " + i + " não colidiu com nada"); // Loga que o raycast não colidiu com nada
                        }
                        else if (raycasts[i].collider != null)
                        {
                            Debug.Log("Raycast " + i + " colidiu com a parede"); // Loga que o raycast colidiu com a parede
                        }
                    }

                    yield return StartCoroutine(SorteioComPorcentagem(opcoesSecundarias));
                    yield break;
                }
                else
                {
                    enemyMove.podeAndar = true;
                    yield break;
                }

                
            }
        }
    }
}
