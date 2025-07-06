using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class EnemyControll : NetworkBehaviour
{
    NetworkObject playerObject;
    [Networked] Vector3 target { get; set; } // Posição alvo do inimigo
    [SerializeField] NetworkCharacterController characterController;

    public override void Spawned()
    {
        characterController = GetComponent<NetworkCharacterController>();

        var playerRefs = Runner.ActivePlayers.ToArray();

        int sorteio = Random.Range(0, playerRefs.Length);

        // Pega o NetworkObject do player sorteado
        PlayerRef playerRef = playerRefs[sorteio];
        playerObject = Runner.GetPlayerObject(playerRef);

        
    }

    public override void FixedUpdateNetwork()
    {
        target = playerObject.transform.position;

        Vector3 direction = (target - transform.position).normalized;
        characterController.Move(direction * characterController.maxSpeed * Runner.DeltaTime);

    }
}
