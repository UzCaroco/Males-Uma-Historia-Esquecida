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

    [SerializeField] UseItem[] useItems; // Lista de itens que o NPC pode usar

    NetworkBool entregouAgua;
    NetworkBool entregouPao;
    NetworkBool entregouTapete;
    NetworkBool entregouCastanhas;

    [SerializeField] GameObject[] itensASerEntregues = new GameObject[4]; // Referência ao NetworkObject que contém os itens a serem entregues

    string[] charadas = new string[4]
    {
        "01.: \"Tenho rumo certo, mas nunca caminho. Sou chão para o sagrado, mas não suporto sapato. Me estendo para quem se curva, e descanso quando termina o chamado.\"",
        "02.: \"Sou feito de muitos, mas ando em círculo. Cada passo que dou é uma lembrança, cada volta, um silêncio que fala alto. Sou pequena, mas carrego grandes nomes.\"",
        "03.: \"Negra como a noite, imóvel como o tempo, sou buscada por milhões que jamais me tocaram. Todos se voltam pra mim, mesmo sem saber o caminho.\"",
        "04.: \"Carrego a ausência plena e a promessa do retorno. Ao meu lado, a centelha imóvel no vazio. Sou lido por quem observa o alto, mas pertenço ao chão das nações. Aponto sem direção, ilumino sem brilhar.\""
    };

    [Networked] int itemCount { get; set; } = 0; // Contador de itens entregues


    public override void Spawned()
    {
        useItems[0].enabled = false; // Desativa a agua
        useItems[1].enabled = false; // Desativa o pao
        useItems[2].enabled = false; // Desativa o tapete
        useItems[3].enabled = false; // Desativa as castanhas
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            AtivarFala(falaInicial); // Ativa a fala inicial
            useItems[0].enabled = true; // Ativa a agua

            if (itensASerEntregues[0] != null)
            {
                itensASerEntregues[0].SetActive(true); // Ativa os itens a serem entregues
            }
        }

        

        else if (entregouAgua && playerInventory.itemAtual == null && itemCount == 1)
        {
            Debug.Log("Pediu o pão");
            AtivarFala(falaPedirPao); // Pede o pão
            useItems[1].enabled = true; //Ativa o pao

            if (itensASerEntregues[1] != null)
            {
                itensASerEntregues[1].SetActive(true); // Ativa os itens a serem entregues
            }
        }
       

        else if (entregouPao && playerInventory.itemAtual == null && itemCount == 2)
        {
            Debug.Log("Pediu o tapete");
            AtivarFala(falaPedirTapete); // Pede o tapete
            useItems[2].enabled = true; // Ativa o tapete

            if (itensASerEntregues[2] != null)
            {
                itensASerEntregues[2].SetActive(true); // Ativa os itens a serem entregues
            }
        }
        

        else if (entregouTapete && playerInventory.itemAtual == null && itemCount == 3)
        {
            Debug.Log("Pediu as castanhas");
            AtivarFala(falaPedirCastanhas); // Pede as castanhas
            useItems[3].enabled = true; // Ativa as castanhas

            if (itensASerEntregues[3] != null)
            {
                itensASerEntregues[3].SetActive(true); // Ativa os itens a serem entregues
            }
        }
        
        else if (playerInventory.itemAtual != null)
        {
            if ((int)playerInventory.itemAtual.itemType == 18 && !entregouAgua) //Se for the agua
            {
                Debug.Log("Entregou a água");
                AtivarFala(falaPrimeiraCharada); // Ativa a fala inicial
                itemCount++;
                entregouAgua = true; // Marca que a água foi entregue

                useItems[0].enabled = false;

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[0]);
            }
            else if ((int)playerInventory.itemAtual.itemType == 8 && entregouAgua) //Se for the paes
            {
                Debug.Log("Entregou o pão");
                AtivarFala(falaSegundaCharada);
                itemCount++;
                entregouPao = true; // Marca que o pão foi entregue

                useItems[1].enabled = false; // Desativa o pão

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[1]);
            }
            else if (entregouPao && (int)playerInventory.itemAtual.itemType == 7) //Se for the tapete
            {
                Debug.Log("Entregou o tapete");
                AtivarFala(falaTerceiraCharada);
                itemCount++;
                entregouTapete = true; // Marca que o tapete foi entregue

                useItems[2].enabled = false; // Desativa o tapete

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[2]);
            }
            else if (entregouTapete && (int)playerInventory.itemAtual.itemType == 9) // Se for castanhas
            {
                Debug.Log("Entregou as castanhas");
                AtivarFala(falaQuartaCharada);
                itemCount++;
                entregouCastanhas = true; // Marca que as castanhas foram entregues

                useItems[3].enabled = false; // Desativa as castanhas

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[3]);
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