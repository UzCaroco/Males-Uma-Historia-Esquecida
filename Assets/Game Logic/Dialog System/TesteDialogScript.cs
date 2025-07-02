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
    string missaoPedidos = "- Buscar Castanhas\r\n- Buscar Tapetes\r\n- Buscar P�es\r\n- Livros do Alcor�o";
    string encontrarChaves = "Encontre as chaves douradas";
    string missoesFase2 = "- Liberte os prisioneiros\r\n- Busque armamentos";
    [Networked] int itemCount { get; set; } = 0; // Contador de itens entregues
    [SerializeField] GameObject[] itensASerEntregues = new GameObject[4]; // Refer�ncia ao NetworkObject que cont�m os itens a serem entregues
    [SerializeField] GameObject canvasLuiza;

    [SerializeField] bool fase2 = false; // Flag para verificar se o jogador j� interagiu com o NPC
    [SerializeField] GameObject canvasCronometro; // Refer�ncia ao NetworkObject do canvas do cron�metro

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            AtivarFala(falaInicial); // Ativa a fala inicial


            if (fase2)
            {
                StartCoroutine(WaitForDialogEnd()); // Espera o di�logo terminar antes de despawnar o NPC
                playerInventory.RPC_AtivarMissoes(missoesFase2); // Chama o RPC para ativar as miss�es
                return;
            }

            playerInventory.RPC_AtivarMissoes(missaoPedidos); // Chama o RPC para ativar as miss�es

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

        if ((int)playerInventory.itemAtual.itemType == 17) // Se for o alcor�o
        {
            AtivarFala(falaAlcorao);
            itemCount++;
        }

        if (itemCount == 4)
        {
            playerInventory.RPC_AtivarMissoes(encontrarChaves); // Chama o RPC para ativar as miss�es
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
        // Espera o di�logo terminar
        yield return new WaitWhile(() => audioSource.isPlaying);

        Runner.Despawn(Object); // Despawns the NPC after the dialog ends
        if (canvasCronometro != null)
        {
            canvasCronometro.SetActive(true); // Ativa o canvas do cron�metro
        }
    }
}
