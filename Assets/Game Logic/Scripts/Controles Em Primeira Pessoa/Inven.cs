
using Fusion;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Inven : NetworkBehaviour
{
    public Camera cam;
    [SerializeField] LayerMask interactableLayer;

    public RaycastHit hitInteract => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    public Transform dropPoint;
    public Sprite itemIcon;
    public ItemData itemAtual = null;
    [Networked] public int itemAtualID { get; set; } = -1; // ID do item atual, -1 significa que n�o h� item selecionado

    public float rayDistance = 100f;

    FirstPersonCamera cameraPessoal;
    PlayerMovement playerMovement;
    public NetworkObject networkObjectInterativo;

    [SerializeField] NetworkCharacterController controller;
    [SerializeField] NetworkObject lamparina;

    public override void Spawned()
    {
        cam = Camera.main;
        cameraPessoal = cam.GetComponent<FirstPersonCamera>();
        controller = GetComponent<NetworkCharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_HandleHit(Vector3 origem, Vector3 direcao)
    {
        Debug.Log("ENTROU AQUI, AGR VAI");

        foreach (var x in Runner.ActivePlayers)
        {
            var networkObject = Runner.GetPlayerObject(x); //Percorre os objetos de rede ativos (Players)
            if (networkObject != null) //Verifica se o objeto de rede n�o � nulo
            {
                if (networkObject.HasStateAuthority)
                {
                    Debug.Log("Encontrou o jogador com autoridade de estado: " + networkObject);


                    // Pega o componente Inven a partir do NetworkObject do player
                    var inven = networkObject.GetComponent<Inven>();
                    if (inven != null)
                    {
                        inven.RPC_AtirarRayCast(origem, direcao, this); // Chama o RPC
                    }

                    break; // Encerra o loop se encontrar o jogador com autoridade de estado
                }
            }
        }
    }
    public override void FixedUpdateNetwork()
    {
        cameraPessoal.UpdateInteragir();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtirarRayCast(Vector3 origem, Vector3 direcao, Inven inventario)
    {
        Debug.Log("Atirando RayCast");
        // Cria o ray com a origem e dire��o recebidas
        Ray ray = new Ray(origem, direcao);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out NetworkObject netObj))
            {
                if (netObj.TryGetComponent(out PickUpItem pickUpItem))
                { 
                    if (pickUpItem != null)
                    {
                        if (pickUpItem.enabled)
                        {
                            if (inventario.itemAtual == null)
                            {
                                if (netObj.TryGetComponent(out IInteractable interactable))
                                {
                                    interactable.RPC_OnInteractObject(inventario); // Chama o m�todo de intera��o do objeto
                                }

                                if ((int)pickUpItem.itemData.itemType != 16)
                                {
                                    inventario.itemAtual = pickUpItem.itemData;
                                    inventario.itemAtualID = (int)pickUpItem.itemData.itemType; // Atualiza o ID do item atual
                                    inventario.itemIcon = pickUpItem.itemData.icon; // Atualiza o �cone do item
                                    inventario.cam.GetComponent<FirstPersonCamera>().slotItem.sprite = pickUpItem.itemData.icon; // Atualiza o �cone do item na c�mera
                                    inventario.dropPoint = hit.transform; // Armazena a posi��o do item
                                }
                                else
                                {
                                    inventario.itemAtualID = (int)pickUpItem.itemData.itemType; // Atualiza o ID do item atual
                                    inventario.itemIcon = pickUpItem.itemData.icon; // Atualiza o �cone do item
                                    inventario.cam.GetComponent<FirstPersonCamera>().slotItem.sprite = pickUpItem.itemData.icon; // Atualiza o �cone do item na c�mera
                                    inventario.dropPoint = hit.transform; // Armazena a posi��o do item
                                }

                            }
                            else
                            {
                                Debug.Log("Invent�rio Cheio");
                            }
                        }
                    }
                }



                else // Se o objeto interativo n�o for um PickUpItem, PEGA TODOS OS COMPONENTES IInteractable
                {
                    IInteractable[] interacoes = netObj.GetComponents<IInteractable>();

                    foreach (var interactable in interacoes)
                    {
                        interactable.RPC_OnInteractObject(inventario);
                    }

                    inventario.networkObjectInterativo = netObj; // Armazena uma vez s�
                }
            }

            else if (inventario.itemAtual != null)
            {
                RPC_AtivarSomDeDrop(); // Ativa o som de drop

                Runner.Spawn(inventario.itemAtual.itemPrefab, hit.point, Quaternion.identity, inputAuthority: Runner.LocalPlayer);
                inventario.RPC_ResetValues(); // Reseta os valores do invent�rio ap�s soltar o item
            }

        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ResetValues()
    {
        itemAtual = null; // Reseta o item atual
        itemAtualID = -1; // Reseta o ID do item atual
        itemIcon = null; // Reseta o �cone do item
        cam.GetComponent<FirstPersonCamera>().slotItem.sprite = null; // Reseta o �cone na c�mera
        dropPoint = null; // Reseta o ponto de queda
    }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtivarChestCode()
    {
        if (cam != null)
        {
            cam.GetComponent<FirstPersonCamera>().AtivarChestCode(); // Ativa o c�digo do ba� se a c�mera estiver definida
        }
    }


    public void RPC_VerificarChestCode(string resposta)
    {
        Debug.Log("entrou no rpc do inventario");
        if (networkObjectInterativo != null && networkObjectInterativo.TryGetComponent(out ChestPadlock chestPadlock))
        {
            Debug.Log("Verificando c�digo do ChestPadlock: " + resposta);
            chestPadlock.RPC_VerificarCodigo(resposta); // Chama o m�todo de verifica��o do c�digo no ChestPadlock
        }
        else
        {
            Debug.LogWarning("NetworkObject interativo n�o encontrado ou n�o possui o componente ChestPadlock.");
        }
    }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtivarLivro()
    {
        if (cam != null)
        {
            cam.GetComponent<FirstPersonCamera>().AtivarLivro(); // Ativa o c�digo do ba� se a c�mera estiver definida
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtivarFrase()
    {
        if (cam != null)
        {
            cam.GetComponent<FirstPersonCamera>().AtivarFrase(); // Ativa o c�digo do ba� se a c�mera estiver definida
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtivarMissoes(string missao)
    {
        if (cam != null)
        {
            cam.GetComponent<FirstPersonCamera>().AtivarMissoes(missao); // Ativa o c�digo do ba� se a c�mera estiver definida
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_EntrarNoArmario(Vector3 posicao)
    {
        Debug.Log("Entrando do arm�rio na posi��o: " + posicao);
        controller.Teleport(posicao); // Teleporta o jogador para a posi��o do arm�rio
        playerMovement.RPC_Agachar(); // Chama o m�todo de agachar no PlayerMovement
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SairDoArmario(Vector3 posicao)
    {
        Debug.Log("Saindo do arm�rio na posi��o: " + posicao);
        controller.Teleport(posicao); // Teleporta o jogador para a posi��o de sa�da do arm�rio
        playerMovement.RPC_Levantar(); // Chama o m�todo de levantar no PlayerMovement
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AdicionarNovoTextoDaCharada(string charada)
    {
        if (cam != null)
        {
            cam.GetComponent<TrocarEnigma>().NovaCharada(charada); // Adiciona um novo texto da charada se a c�mera estiver definida
        }
    }













    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtivarSomDePickUp()
    {
        if (cam != null)
        {
            if (cam.TryGetComponent<FxSounds>(out FxSounds fxSounds))
            {
                fxSounds.PlayPickUpSound(); // Ativa o som de pick up se o componente FxSounds estiver presente na c�mera
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AtivarSomDeDrop()
    {
        if (cam != null)
        {
            if (cam.TryGetComponent<FxSounds>(out FxSounds fxSounds))
            {
                fxSounds.PlayDropSound(); // Ativa o som de drop se o componente FxSounds estiver presente na c�mera
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_MissaoConcluidaTeleportePlayer()
    {
        Debug.Log("Miss�o conclu�da, teleportando jogador para a posi��o de conclus�o.");
        GetComponent<PlayerMovement>().RPC_DeathAndRespawnPlayer(new Vector3(-18.4103f, 7.5825f, 13.5f));
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SpawnarLamparinaComRaio(Vector3 posicao)
    {
        Runner.Spawn(lamparina, posicao, Quaternion.identity, inputAuthority: Runner.LocalPlayer);
    }
}
