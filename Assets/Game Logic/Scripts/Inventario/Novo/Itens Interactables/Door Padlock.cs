using System.Collections;
using Fusion;
using UnityEngine;

public class DoorPadlock : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;
    [SerializeField] NetworkObject doorLeft, doorRight;


    [Networked][SerializeField] int locksOpens { get; set; }

    public override void Spawned()
    {

    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OpenLock()
    {
        Debug.Log("Tentando abrir a porta com a fechadura");
        if (locksOpens < 2)
        {
            Debug.Log("A porta ainda está fechada, você precisa de mais chaves para abri-la.");
            locksOpens++;
            Debug.Log("quantidade abertas" + locksOpens);
        }
        else
        {
            // Spawna a segunda fase
            foreach (var x in Runner.ActivePlayers)
            {
                var networkObject = Runner.GetPlayerObject(x); //Percorre os objetos de rede ativos (Players)
                if (networkObject != null) //Verifica se o objeto de rede não é nulo
                {
                    if (networkObject.HasStateAuthority)
                    {
                        Debug.Log("Encontrou o jogador com autoridade de estado: " + networkObject);

                        // Pega o componente Inven a partir do NetworkObject do player
                        var hostSpawnPhase = networkObject.GetComponent<SpawnNewPhase>();
                        if (hostSpawnPhase != null)
                        {
                            Debug.Log("Chamando RPC para spawnar nova fase no jogador:");
                            hostSpawnPhase.RPC_SpawnPhase(); // Chama o RPC para spawnar a nova fase
                        }

                        break; // Encerra o loop se encontrar o jogador com autoridade de estado
                    }
                }
            }



            // Abre as portas
            open = true;

            Debug.Log("Porta aberta com sucesso!");
            doorLeft.transform.Rotate(0, 0, -120f);
            doorRight.transform.Rotate(0, 0, 120f);

            Runner.Despawn(Object); // Despawna a corrente e o cadeado





            
        }
    }
}
