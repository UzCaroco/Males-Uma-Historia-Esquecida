using Fusion;
using UnityEngine;

public class Patrol : NetworkBehaviour
{
    CharacterController cControler;

    [SerializeField] float velocity = 5, velocityRotation;
    public Transform waypoint;
    public Transform pointLast;

    public NetworkBool walk = true, lookPlayer = false;

    public Transform playerEncontrado;

    private void Start()
    {
        cControler = GetComponent<CharacterController>();
    }
    public override void Spawned()
    {
        cControler = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!lookPlayer)
        {
            Vector3 targetRotation = Quaternion.LookRotation(waypoint.position - transform.position).eulerAngles;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), velocityRotation);

            if (walk)
            {
                cControler.Move((waypoint.position - transform.position).normalized * Runner.DeltaTime * velocity);
            }
        }
        else
        {
            Vector3 targetRotation = Quaternion.LookRotation(playerEncontrado.position - transform.position).eulerAngles;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), velocityRotation);

            if (walk)
            {
                cControler.Move((playerEncontrado.position - transform.position).normalized * Runner.DeltaTime * velocity);
            }
        }
    }


}
