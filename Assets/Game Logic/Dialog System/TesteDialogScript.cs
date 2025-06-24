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

    [Networked] int itemCount { get; set; } = 0; // Contador de itens entregues

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            AtivarFala(falaInicial); // Ativa a fala inicial

            playerInventory.RPC_AtivarMissoes(); // Chama o RPC para ativar as missões
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
            audioSource.clip = entregaDeTodosOsItens;
            audioSource.Play();
        }
    }
}
