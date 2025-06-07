
using Fusion;
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
    [Networked] public int itemAtualID { get; set; } = -1; // ID do item atual, -1 significa que não há item selecionado

    public float rayDistance = 100f;

    FirstPersonCamera cameraPessoal;
    

    public override void Spawned()
    {
        cam = Camera.main;
        cameraPessoal = cam.GetComponent<FirstPersonCamera>();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_HandleHit(Vector3 origem, Vector3 direcao)
    {
        Debug.Log("ENTROU AQUI, AGR VAI");

        foreach (var x in Runner.ActivePlayers)
        {
            var networkObject = Runner.GetPlayerObject(x); //Percorre os objetos de rede ativos (Players)
            if (networkObject != null) //Verifica se o objeto de rede não é nulo
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
        // Cria o ray com a origem e direção recebidas
        Ray ray = new Ray(origem, direcao);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out NetworkObject netObj))
            {
                if (netObj.TryGetComponent(out PickUpItem pickUpItem))
                {
                    if (pickUpItem != null)
                    {
                        if (inventario.itemAtual == null)
                        {
                            inventario.itemAtual = pickUpItem.itemData;
                            inventario.itemAtualID = (int)pickUpItem.itemData.itemType; // Atualiza o ID do item atual
                            inventario.itemIcon = pickUpItem.itemData.icon; // Atualiza o ícone do item
                            inventario.cam.GetComponent<FirstPersonCamera>().slotItem.sprite = pickUpItem.itemData.icon; // Atualiza o ícone do item na câmera
                            inventario.dropPoint = hit.transform; // Armazena a posição do item

                        }
                        else
                        {
                            Debug.Log("Inventário Cheio");
                        }
                    }
                }

                

                if (netObj.TryGetComponent(out IInteractable interactable))
                {
                    interactable.RPC_OnInteractObject(inventario);
                }
            }

            else if (inventario.itemAtual != null)
            {
                Runner.Spawn(inventario.itemAtual.itemPrefab, hit.point, Quaternion.identity, inputAuthority: Runner.LocalPlayer);
                inventario.RPC_ResetValues(); // Reseta os valores do inventário após soltar o item
            }

        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ResetValues()
    {
        itemAtual = null; // Reseta o item atual
        itemAtualID = -1; // Reseta o ID do item atual
        itemIcon = null; // Reseta o ícone do item
        cam.GetComponent<FirstPersonCamera>().slotItem.sprite = null; // Reseta o ícone na câmera
        dropPoint = null; // Reseta o ponto de queda
    }

}
