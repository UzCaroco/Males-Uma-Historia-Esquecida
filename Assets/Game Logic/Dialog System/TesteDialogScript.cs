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
    [Networked] int itemCount { get; set; } = 0; // Contador de itens entregues
    [SerializeField] GameObject[] itensASerEntregues = new GameObject[4]; // Refer�ncia ao NetworkObject que cont�m os itens a serem entregues
    [SerializeField] GameObject canvasLuiza;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            AtivarFala(falaInicial); // Ativa a fala inicial

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
}
