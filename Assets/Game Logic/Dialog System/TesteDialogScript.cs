using System.Collections;
using cherrydev;
using Fusion;
using UnityEngine;

public class TesteDialogScript : NetworkBehaviour, IInteractable
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogNodeGraph;
    [SerializeField] AudioSource audioSource;

    NetworkBool dialogoInicial = false;

    [SerializeField] AudioClip falaInicial; // Fala inicial do NPC
    [SerializeField] AudioClip falaPaes;
    [SerializeField] AudioClip falaTapete;
    [SerializeField] AudioClip falaCastanhas;
    [SerializeField] AudioClip falaAlcorao;
    [SerializeField] AudioClip entregaDeTodosOsItens;
    string missaoPedidos = "- Buscar Castanhas\r\n- Buscar Tapetes\r\n- Buscar Pães\r\n- Livros do Alcorão";
    string encontrarChaves = "Encontre as chaves douradas";
    string missoesFase2 = "- Liberte os prisioneiros\r\n- Busque armamentos";
    string missoesFase3 = "- Derrote o máximo de inimigos";
    [Networked] int itemCount { get; set; } = 0; // Contador de itens entregues
    [SerializeField] GameObject[] itensASerEntregues = new GameObject[4]; // Referência ao NetworkObject que contém os itens a serem entregues
    [SerializeField] GameObject canvasLuiza;

    [SerializeField] bool fase2 = false, fase3; // Flag para verificar se o jogador já interagiu com o NPC

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            AtivarFala(falaInicial); // Ativa a fala inicial


            if (fase2)
            {
                StartCoroutine(WaitForDialogEnd()); // Espera o diálogo terminar antes de despawnar o NPC
                playerInventory.RPC_AtivarMissoes(missoesFase2); // Chama o RPC para ativar as missões
                return;
            }
            else if (fase3)
            {
                StartCoroutine(WaitForDialogEnd());
                playerInventory.RPC_AtivarMissoes(missoesFase3); // Chama o RPC para ativar as missões
                return;
            }

                playerInventory.RPC_AtivarMissoes(missaoPedidos); // Chama o RPC para ativar as missões

            for (int i = 0; i < itensASerEntregues.Length; i++)
            {
                if (itensASerEntregues[i] != null)
                {
                    itensASerEntregues[i].SetActive(true); // Ativa os itens a serem entregues
                }
            }
        }

        if ((int)playerInventory.itemAtual.itemType == 7) //Se for the tapete
        {
            AtivarFala(falaTapete); // Ativa a fala inicial
            itemCount++;
        }

        if ((int)playerInventory.itemAtual.itemType == 8) //Se for the paes
        {
            AtivarFala(falaPaes);
            itemCount++;
        }

        if ((int)playerInventory.itemAtual.itemType == 9) //Se for the castanhas
        {
            AtivarFala(falaCastanhas);
            itemCount++;
        }

        if ((int)playerInventory.itemAtual.itemType == 17) // Se for o alcorão
        {
            AtivarFala(falaAlcorao);
            itemCount++;
        }

        if (itemCount == 4)
        {
            playerInventory.RPC_AtivarMissoes(encontrarChaves); // Chama o RPC para ativar as missões
        }


    }

    private void AtivarFala(AudioClip audio)
    {
        dialogBehaviour.StartDialog(dialogNodeGraph);

        StartCoroutine(TocarFala(audio));
    }

    private IEnumerator TocarFala(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();

        // Espera o som terminar
        yield return new WaitWhile(() => audioSource.isPlaying);

        if (itemCount == 4)
        {
            if (canvasLuiza != null)
            {
                canvasLuiza.SetActive(false); // Ativa o canvas da Luiza
            }
            audioSource.clip = entregaDeTodosOsItens;
            audioSource.Play();
        }
    }

    IEnumerator WaitForDialogEnd()
    {
        // Espera o diálogo terminar
        yield return new WaitWhile(() => audioSource.isPlaying);

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
                        if (fase2)
                        {
                            Debug.Log("Chamando RPC para spawnar nova fase no jogador:");
                            hostSpawnPhase.RPC_SpawnTemporizador(); // Chama o RPC para spawnar o temporizador
                        }
                        else if (fase3)
                        {
                            Debug.Log("Chamando RPC para spawnar nova fase no jogador:");
                            hostSpawnPhase.RPC_ManagerFase3(); // Chama o RPC para spawnar o temporizador
                        }
                        
                    }

                    break; // Encerra o loop se encontrar o jogador com autoridade de estado
                }
            }
        }

        Runner.Despawn(Object); // Despawns the NPC after the dialog ends
    }
}
