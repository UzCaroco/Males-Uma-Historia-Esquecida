using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SpawnNewPhase : NetworkBehaviour
{
    [SerializeField] sbyte phaseToSpawn = 0;
    [SerializeField] NetworkObject[] phasePrefab = new NetworkObject[2];



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SpawnPhase()
    {
        Debug.Log("Chamando RPC para spawnar nova fase no jogador: ");
        Runner.Spawn(phasePrefab[phaseToSpawn], inputAuthority: Runner.LocalPlayer);

        phaseToSpawn++;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DespawnLastPhase()
    {
        Debug.Log("Chamando RPC para despawnar a fase anterior: ");
        if (phaseToSpawn == 1)
        {
            NetworkObject networkObject = GameObject.Find("Sobrado(Clone)").GetComponent<NetworkObject>();
            Debug.Log("BUSCANDO");
            if (networkObject != null)
            {
                
                Runner.Despawn(networkObject);
                Debug.Log("Despawned Sobrado");
            }
        }
    }
}
