using Fusion;
using UnityEngine;

public class SpawnNewPhase : NetworkBehaviour
{
    [SerializeField] sbyte phaseToSpawn = 0;
    [SerializeField] NetworkObject[] phasePrefab = new NetworkObject[2];

    [SerializeField] NetworkObject canvasTemporizador, managerFase3;


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SpawnPhase()
    {
        Debug.Log("Chamando RPC para spawnar nova fase:" + phaseToSpawn);
        Runner.Spawn(phasePrefab[phaseToSpawn], inputAuthority: Runner.LocalPlayer);

        phaseToSpawn++;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DespawnLastPhase()
    {
        Debug.Log("Chamando RPC para despawnar a fase anterior: ");
        if (phaseToSpawn == 1)
        {
            NetworkObject networkObject = GameObject.Find("Bake Sobrado(Clone)").GetComponent<NetworkObject>();
            Debug.Log("BUSCANDO");
            if (networkObject != null)
            {
                
                Runner.Despawn(networkObject);
                Debug.Log("Despawned Sobrado");
            }
        }
        else if (phaseToSpawn == 2)
        {
            NetworkObject networkObject = GameObject.Find("Bake Camara(Clone)").GetComponent<NetworkObject>();
            Debug.Log("BUSCANDO FASE 2" + networkObject);
            if (networkObject != null)
            {
                Runner.Despawn(networkObject);
                Debug.Log("Despawned Sobrado Fase 2");
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SpawnTemporizador()
    {
        Debug.Log("Chamando RPC para spawnar o temporizador: ");
        if (canvasTemporizador != null)
        {
            Runner.Spawn(canvasTemporizador, inputAuthority: Runner.LocalPlayer);
            Debug.Log("Spawned Temporizador");
        }
        else
        {
            Debug.LogError("Canvas Temporizador não está definido!");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ManagerFase3()
    {
        Debug.Log("Chamando RPC para spawnar o gamemanager: ");
        if (canvasTemporizador != null)
        {
            Runner.Spawn(managerFase3, inputAuthority: Runner.LocalPlayer);
            Debug.Log("Spawned Temporizador");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RestartFase2()
    {
        Debug.Log("Chamando RPC para reiniciar a fase 2: ");

        NetworkObject networkObject = GameObject.Find("Bake Camara(Clone)").GetComponent<NetworkObject>();
        Debug.Log("BUSCANDO FASE 2" + networkObject);
        if (networkObject != null)
        {
            Runner.Despawn(networkObject);
            Debug.Log("Despawned Sobrado Fase 2");
        }

        Runner.Spawn(phasePrefab[0], inputAuthority: Runner.LocalPlayer);

        RPC_SpawnTemporizador(); // Spawns the timer

        foreach (var x in Runner.ActivePlayers)
        {
            var obj = Runner.GetPlayerObject(x); //Percorre os objetos de rede ativos (Players)
            if (obj != null) //Verifica se o objeto de rede não é nulo
            {
                Debug.Log("Encontrou o jogador com autoridade de estado: " + obj);

                // Pega o componente Inven a partir do NetworkObject do player
                var hostSpawnPhase = obj.GetComponent<PlayerMovement>();
                if (hostSpawnPhase != null)
                {
                    Debug.Log("Chamando RPC para spawnar nova fase no jogador:");
                    hostSpawnPhase.RPC_DeathAndRespawnPlayer(new Vector3(-18.4103f, 7.5825f, 13.5f)); // Chama o RPC para spawnar o temporizador
                }
            }
        }
    }

    public void VerificarPlayerComAutoridade() //So entra aqui quando for da segunda para a terceira fase
    {
        foreach (var x in Runner.ActivePlayers)
        {
            var networkObject = Runner.GetPlayerObject(x); //Percorre os objetos de rede ativos (Players)
            if (networkObject != null) //Verifica se o objeto de rede não é nulo
            {
                if (networkObject.HasStateAuthority)
                {
                    if (networkObject.TryGetComponent<SpawnNewPhase>(out var spawnPhase))
                    {
                        Debug.Log("Encontrou o jogador com autoridade de estado: " + networkObject);
                        spawnPhase.RPC_SpawnPhase(); // Chama o RPC para spawnar a nova fase
                        spawnPhase.RPC_DespawnLastPhase();
                    }
                    break; // Encerra o loop se encontrar o jogador com autoridade de estado
                }
            }
        }

    }
}
