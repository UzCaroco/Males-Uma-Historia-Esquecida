using Fusion;
using UnityEngine;

public class Patrol : NetworkBehaviour
{
    CharacterController cControler;

    [SerializeField] float velocity = 5, velocityRotation;
    public Transform waypoint;
    public Transform pointLast;

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
        Vector3 targetRotation = Quaternion.LookRotation(waypoint.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), velocityRotation);

        cControler.Move((waypoint.position - transform.position).normalized * Runner.DeltaTime * velocity);
    }
}
