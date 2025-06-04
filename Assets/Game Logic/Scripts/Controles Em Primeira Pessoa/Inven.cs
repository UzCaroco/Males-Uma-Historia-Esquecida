
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class Inven : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask interactableLayer;

    public RaycastHit hitInteract => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    public Transform dropPoint;
    public Image itemIcon;
    public ItemData itemAtual = null;

    public float rayDistance = 100f;

    FirstPersonCamera cameraPessoal;
    void Start()
    {
        Debug.Log($"Start - Tem Input Authority? {Object.HasInputAuthority}");
        Debug.Log($"Start - Tem State Authority? {Object.HasStateAuthority}");
    }

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
                        inven.RPC_AtirarRayCast(origem, direcao); // Chama o RPC
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
    public void RPC_AtirarRayCast(Vector3 origem, Vector3 direcao)
    {
        Debug.Log("Atirando RayCast");
        // Cria o ray com a origem e direção recebidas
        Ray ray = new Ray(origem, direcao);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out NetworkObject netObj))
            {
                if (netObj.TryGetComponent(out IInteractable interactable))
                {
                    interactable.RPC_OnInteractObject(this);
                }
            }
        }
    }

}
