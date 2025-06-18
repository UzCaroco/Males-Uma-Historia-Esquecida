using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TriggerSpawnNewPhase : NetworkBehaviour
{
    [Networked][SerializeField] int playersQuePassaram { get; set; } = 0;
    [SerializeField] sbyte quantidadeDePlayersParaSpawnarNovaPhase = 1; // Quantidade de players que precisam passar pelo trigger para spawnar a nova fase

    NetworkBool isTriggerActive = false; // Variável para controlar se o trigger está ativo

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggerActive)
        {
            isTriggerActive = true; // Ativa o trigger para evitar múltiplas ativações

            Debug.Log("Player entrou no trigger");
            playersQuePassaram++;
            if (playersQuePassaram >= quantidadeDePlayersParaSpawnarNovaPhase)
            {


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
                                hostSpawnPhase.RPC_DespawnLastPhase(); // Chama o RPC para despawnar a fase anterior
                            }

                            break; // Encerra o loop se encontrar o jogador com autoridade de estado
                        }
                    }
                }



            }
        }
    }
}
