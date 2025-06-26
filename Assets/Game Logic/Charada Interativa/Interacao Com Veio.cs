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

    string[] charadas = new string[4]
    {
        "01.: \"Tenho rumo certo, mas nunca caminho. Sou ch�o para o sagrado, mas n�o suporto sapato. Me estendo para quem se curva, e descanso quando termina o chamado.\"",
        "02.: \"Sou feito de muitos, mas ando em c�rculo. Cada passo que dou � uma lembran�a, cada volta, um sil�ncio que fala alto. Sou pequena, mas carrego grandes nomes.\"",
        "03.: \"Negra como a noite, im�vel como o tempo, sou buscada por milh�es que jamais me tocaram. Todos se voltam pra mim, mesmo sem saber o caminho.\"",
        "04.: \"Carrego a aus�ncia plena e a promessa do retorno. Ao meu lado, a centelha im�vel no vazio. Sou lido por quem observa o alto, mas perten�o ao ch�o das na��es. Aponto sem dire��o, ilumino sem brilhar.\""
    };

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
            Debug.Log("Pediu o p�o");
            AtivarFala(falaPedirPao); // Pede o p�o
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
                Debug.Log("Entregou a �gua");
                AtivarFala(falaPrimeiraCharada); // Ativa a fala inicial
                itemCount++;
                entregouAgua = true; // Marca que a �gua foi entregue

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[0]);
            }
            else if ((int)playerInventory.itemAtual.itemType == 8 && entregouAgua) //Se for the paes
            {
                Debug.Log("Entregou o p�o");
                AtivarFala(falaSegundaCharada);
                itemCount++;
                entregouPao = true; // Marca que o p�o foi entregue

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[1]);
            }
            else if (entregouPao && (int)playerInventory.itemAtual.itemType == 7) //Se for the tapete
            {
                Debug.Log("Entregou o tapete");
                AtivarFala(falaTerceiraCharada);
                itemCount++;
                entregouTapete = true; // Marca que o tapete foi entregue

                playerInventory.RPC_AdicionarNovoTextoDaCharada(charadas[2]);
            }
            else if (entregouTapete && (int)playerInventory.itemAtual.itemType == 9) // Se for castanhas
            {
                Debug.Log("Entregou as castanhas");
                AtivarFala(falaQuartaCharada);
                itemCount++;
                entregouCastanhas = true; // Marca que as castanhas foram entregues

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