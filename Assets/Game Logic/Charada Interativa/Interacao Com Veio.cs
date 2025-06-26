using System.Collections;
using System.Collections.Generic;
using cherrydev;
using Fusion;
using UnityEngine;
using UnityEngine.Rendering;

public class InteracaoComVeio : NetworkBehaviour, IInteractable
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogNodeGraph;
    [SerializeField] AudioSource audioSource;

    NetworkBool dialogoInicial = false;

    [SerializeField] AudioClip falaInicial; // Fala inicial do NPC
    [SerializeField] AudioClip falaPedirAgua;
    [SerializeField] AudioClip falaPrimeiraCharada;
    [SerializeField] AudioClip falaPedirPao;
    [SerializeField] AudioClip falaSegundaCharada;
    [SerializeField] AudioClip falaPedirTapete;
    [SerializeField] AudioClip falaTerceiraCharada;
    [SerializeField] AudioClip falaPedirCastanhas;
    [SerializeField] AudioClip falaQuartaCharada;

    NetworkBool entregouAgua;
    NetworkBool entregouPao;
    NetworkBool entregouTapete;
    NetworkBool entregouCastanhas;

    [Networked] int itemCount { get; set; } = 0; // Contador de itens entregues

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            AtivarFala(falaInicial); // Ativa a fala inicial
        }

        

        else if (entregouAgua && playerInventory.itemAtual == null && itemCount == 1)
        {
            Debug.Log("Pediu o pão");
            AtivarFala(falaPedirPao); // Pede o pão
        }
       

        else if (entregouPao && playerInventory.itemAtual == null && itemCount == 2)
        {
            Debug.Log("Pediu o tapete");
            AtivarFala(falaPedirTapete); // Pede o tapete
        }
        

        else if (entregouTapete && playerInventory.itemAtual == null && itemCount == 3)
        {
            Debug.Log("Pediu as castanhas");
            AtivarFala(falaPedirCastanhas); // Pede as castanhas
        }
        
        else if (playerInventory.itemAtual != null)
        {
            if ((int)playerInventory.itemAtual.itemType == 18 && !entregouAgua) //Se for the agua
            {
                Debug.Log("Entregou a água");
                AtivarFala(falaPrimeiraCharada); // Ativa a fala inicial
                itemCount++;
                entregouAgua = true; // Marca que a água foi entregue
            }
            else if ((int)playerInventory.itemAtual.itemType == 8 && entregouAgua) //Se for the paes
            {
                Debug.Log("Entregou o pão");
                AtivarFala(falaSegundaCharada);
                itemCount++;
                entregouPao = true; // Marca que o pão foi entregue
            }
            else if (entregouPao && (int)playerInventory.itemAtual.itemType == 7) //Se for the tapete
            {
                Debug.Log("Entregou o tapete");
                AtivarFala(falaTerceiraCharada);
                itemCount++;
                entregouTapete = true; // Marca que o tapete foi entregue
            }
            else if (entregouTapete && (int)playerInventory.itemAtual.itemType == 9) // Se for castanhas
            {
                Debug.Log("Entregou as castanhas");
                AtivarFala(falaQuartaCharada);
                itemCount++;
                entregouCastanhas = true; // Marca que as castanhas foram entregues
            }
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

        if (audio == falaInicial)
        {
            StartCoroutine(PedirAgua());
        }

        yield return null;

    }

    private IEnumerator PedirAgua()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        if (!entregouAgua)
        {
            AtivarFala(falaPedirAgua);
        }
    }
}