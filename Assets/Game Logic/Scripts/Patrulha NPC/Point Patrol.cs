using System.Collections;
using UnityEngine;
using Fusion;

public class PointPatrol : NetworkBehaviour
{
    [SerializeField] NetworkObject[] waypoints;
    [SerializeField] Patrol enemyPatrol;

    NetworkBool isResetting = false;
    [SerializeField] NetworkObject[] pointsRotateAround = new NetworkObject[2];

    public override void Spawned()
    {
        Debug.Log(Object.name + Object.Id);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isResetting)
        {
            if (!enemyPatrol.HasStateAuthority) return;

            sbyte sorteio = (sbyte)Random.Range(0, 2);

            if (sorteio == 0)
            {
                Debug.Log("sorteou novo ponto de patrulha");
                SortearNovo();
            }
            else
            {
                Debug.Log("sorteou olhar ao redor");
                if (!enemyPatrol.lookPlayer)
                {
                    OlharAoRedor();
                }
                else
                {
                    SortearNovo();
                }
            }

            isResetting = true;
        }
    }

    IEnumerator Resetar()
    {
        yield return new WaitForSeconds(2f);
        isResetting = false;
    }

    void SortearNovo()
    {
        if (waypoints.Length == 1)
        {
            enemyPatrol.WaypointId = waypoints[0].Id;
            enemyPatrol.PointLastId = Object.Id;
        }
        else if (waypoints.Length > 1)
        {
            int x;
            do
            {
                x = Random.Range(0, waypoints.Length);
            } while (waypoints[x].Id == enemyPatrol.PointLastId);

            enemyPatrol.WaypointId = waypoints[x].Id;
            enemyPatrol.PointLastId = Object.Id;
        }

        enemyPatrol.walk = true;
        StartCoroutine(Resetar());
    }

    void OlharAoRedor()
    {
        enemyPatrol.walk = false;
        StartCoroutine(Olhando());
    }

    IEnumerator Olhando()
    {
        enemyPatrol.WaypointId = pointsRotateAround[0].Id;
        yield return new WaitForSeconds(2f);

        enemyPatrol.WaypointId = pointsRotateAround[1].Id;
        yield return new WaitForSeconds(2f);

        SortearNovo();
    }
}