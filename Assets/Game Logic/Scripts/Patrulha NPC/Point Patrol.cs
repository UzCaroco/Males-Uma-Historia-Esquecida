using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPatrol : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;

    [SerializeField] Patrol enemyPatrol;

    bool isResetting = false;
    [SerializeField] Transform[] pointsRotateAround = new Transform[2];

    


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            if (!isResetting)
            {
                sbyte sorteio = (sbyte)Random.Range(0, 2);

                if (sorteio == 0)
                {
                    Debug.Log("sorteiou novo ponto de patrulha");
                    SortearNovo();
                }
                else
                {
                    Debug.Log("sorteiou olhar ao redor");
                    OlharAoRedor();
                }
            }
            isResetting = true;

        }
    }

    IEnumerator Resetar()
    {
        yield return new WaitForSeconds(2f);
        isResetting = false; // Reseta o estado de isResetting após um curto período
    }


    void SortearNovo()
    {
        
        if (waypoints.Length == 1)
        {
            enemyPatrol.waypoint = waypoints[0];
            enemyPatrol.pointLast = gameObject.transform; // Define o ponto atual como o último ponto visitado pelo inimigo
        }
        else if (waypoints.Length > 1)
        {
            int x;
            do
            {
                x = Random.Range(0, waypoints.Length);
            } while (enemyPatrol.pointLast == waypoints[x]); // Enquanto o ponto anterior for igual ao ponto sorteado, sorteia um novo ponto

            enemyPatrol.waypoint = waypoints[x];
            enemyPatrol.pointLast = gameObject.transform; // Define o ponto atual como o último ponto visitado pelo inimigo
        }

        enemyPatrol.walk = true; // Permite o inimigo se mover
        StartCoroutine(Resetar()); // Inicia a coroutine para resetar o estado de isResetting após um curto período
    }

    void OlharAoRedor()
    {
        enemyPatrol.walk = false; // Para o inimigo de se mover enquanto ele olha ao redor

        StartCoroutine(Olhando()); // Inicia a coroutine que faz o inimigo olhar ao redor

    }

    IEnumerator Olhando()
    {
        enemyPatrol.waypoint = pointsRotateAround[0];
        yield return new WaitForSeconds(2f); // Aguarda 2 segundos enquanto o inimigo olha ao redor

        enemyPatrol.waypoint = pointsRotateAround[1];
        yield return new WaitForSeconds(2f); // Aguarda 2 segundos enquanto o inimigo olha ao redor

        
        SortearNovo();
        
    }
}
