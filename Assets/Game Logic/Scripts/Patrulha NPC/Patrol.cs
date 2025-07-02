using Fusion;
using UnityEngine;

public class Patrol : NetworkBehaviour
{
    CharacterController cControler;
    NetworkCharacterController networkCharacterController;

    [SerializeField] float velocity = 5, velocityRotation;
    public Transform waypoint;
    public Transform pointLast;

    public NetworkBool walk = true, lookPlayer = false;

    public Transform playerEncontrado;
    [SerializeField] Transform posicaoDeRespawnPlayer;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip somPassos;

    private void Start()
    {
        cControler = GetComponent<CharacterController>();
        networkCharacterController = GetComponent<NetworkCharacterController>();
    }
    public override void Spawned()
    {
        cControler = GetComponent<CharacterController>();
        networkCharacterController = GetComponent<NetworkCharacterController>();

        
    }

    public override void FixedUpdateNetwork()
    {

        if (!lookPlayer)
        {
            Vector3 targetRotation = Quaternion.LookRotation(waypoint.position - transform.position).eulerAngles;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), velocityRotation);

            if (walk)
            {
                //cControler.Move((waypoint.position - transform.position).normalized * Runner.DeltaTime * velocity);
                cControler.Move((waypoint.position - transform.position).normalized * Runner.DeltaTime * velocity);
            }
        }
        else
        {
            Vector3 targetRotation = Quaternion.LookRotation(playerEncontrado.position - transform.position).eulerAngles;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), velocityRotation);

            if (walk)
            {
                //cControler.Move((playerEncontrado.position - transform.position).normalized * Runner.DeltaTime * velocity);
                cControler.Move((playerEncontrado.position - transform.position).normalized * Runner.DeltaTime * velocity);
            }
        }

        if (transform.position.x > -38.357f)
        {
            lookPlayer = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && lookPlayer)
        {
            if (other.TryGetComponent(out PlayerMovement playerMovement))
            {
                lookPlayer = false;
                waypoint = pointLast;
                playerMovement.RPC_DeathAndRespawnPlayer(posicaoDeRespawnPlayer.position);
            }
        }
    }

    public void SomPasso()
    {
        aud.PlayOneShot(somPassos);
    }

}
