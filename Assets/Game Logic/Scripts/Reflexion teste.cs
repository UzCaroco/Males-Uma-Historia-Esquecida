using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflexionteste : MonoBehaviour
{
    public int maxReflections = 5;
    public float maxDistance = 100f;
    public LineRenderer lineRenderer;

    void Update()
    {
        CastRay();
    }

    void CastRay()
    {
        Vector3 direction = transform.forward;
        Vector3 position = transform.position;

        // Reseta a linha a cada frame para n�o ficar acumulando linhas
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, position);

        for (int i = 0; i < maxReflections; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            // Verifica se bateu em algum colisor
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                // Quando tiver batendo no colisor correto, adiciona uma nova linha na dire��o do hit
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                // Se bateu no espelho calcula a nova dire��o do reflexo
                if (hit.collider.CompareTag("Finish"))
                {
                    direction = Vector3.Reflect(direction, hit.normal); //pega a dire��o do raio e devolve a nova dire��o baseada no �ngulo da superf�cie do espelho.
                    position = hit.point;
                }
                else
                {
                    //// Chegou num destino (tipo a parede falsa)
                    //if (hit.collider.CompareTag("Estante"))
                    //{
                    //    Debug.Log("Acertou o Estante!");

                    //    hit.collider.gameObject.SetActive(false); // Desativa o objeto do livro
                    //    // Aqui voc� chama o m�todo pra abrir a parede
                    //}
                    break;
                }
            }
            else // Desenha a �ltima linha, mesmo se essa linha n�o batendo em nada
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, position + direction * maxDistance);
                break;
            }
        }
    }
}
