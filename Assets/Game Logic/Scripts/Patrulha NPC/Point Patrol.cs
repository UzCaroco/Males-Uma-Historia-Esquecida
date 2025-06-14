using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPatrol : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;

    [SerializeField] Patrol enemyPatrol;

    bool isResetting = false;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            if (!isResetting)
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
                        Debug.Log("Ponto anterior: " + enemyPatrol.pointLast + " novo ponto " + waypoints[x].transform); // Log do ponto sorteado para debug
                    } while (enemyPatrol.pointLast == waypoints[x]); // Enquanto o ponto anterior for igual ao ponto sorteado, soreta um novo ponto

                    enemyPatrol.waypoint = waypoints[x];
                    enemyPatrol.pointLast = gameObject.transform; // Define o ponto atual como o último ponto visitado pelo inimigo
                }
            }

            isResetting = true;
            StartCoroutine(Resetar()); // Inicia a coroutine para resetar o estado de isResetting após um curto período


        }
    }

    IEnumerator Resetar()
    {
        yield return new WaitForSeconds(1f);
        isResetting = false; // Reseta o estado de isResetting após um curto período
    }
}
