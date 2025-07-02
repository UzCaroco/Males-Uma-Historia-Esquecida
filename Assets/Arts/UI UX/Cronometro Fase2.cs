using System.Collections;
using TMPro;
using UnityEngine;
using Fusion;

public class CronometroFase2 : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI textoTempo;
    [Networked] [SerializeField] int tempoLimite { get; set; } = 600; // Tempo limite em segundos (2 minutos)
    void Start()
    {
        StartCoroutine(Temporizador()); // Inicia o temporizador
    }


    IEnumerator Temporizador()
    {
        while (tempoLimite > 0)
        {
            yield return new WaitForSeconds(1);
            tempoLimite--;
            AtualizarTexto();
        }

        if (tempoLimite <= 0)
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
                            hostSpawnPhase.RPC_RestartFase2(); // Chama o RPC para spawnar o temporizador
                        }

                        break; // Encerra o loop se encontrar o jogador com autoridade de estado
                    }
                }
            }
        }
    }

    void AtualizarTexto()
    {
        int minutos = tempoLimite / 60;
        int segundos = tempoLimite % 60;
        textoTempo.text = string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }
}
