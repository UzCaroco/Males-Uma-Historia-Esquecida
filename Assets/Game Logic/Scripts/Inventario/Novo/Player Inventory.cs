using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventory : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask interactableLayer;

    public RaycastHit hitInteract => Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    public Transform dropPoint;
    public Image itemIcon;
    public ItemData itemAtual = null;

    [SerializeField] float tempoPressionado = 0f, limiteMaxParaPointerDownIniciar = 5f;

    [SerializeField] private NetworkObject prefabGameManager;



    public float rayDistance = 100f;

    void Start()
    {
        Debug.Log($"Start - Tem Input Authority? {Object.HasInputAuthority}");
        Debug.Log($"Start - Tem State Authority? {Object.HasStateAuthority}");
    }

    public override void Spawned()
    {
        Debug.Log("Spawned do PlayerInventory");
        if (HasInputAuthority)
        {
            Debug.Log("parece q essa porra foi");
            Runner.SetPlayerObject(Object.InputAuthority, Object); // Set the player object for the input authority









            PlayerSpawn[] spawns = FindObjectsByType<PlayerSpawn>(FindObjectsSortMode.None);

            if (Object.HasStateAuthority)
            {
                foreach (var spawn in spawns)
                {

                    if (spawn != this && spawn.gameObject.name == "Prototype Runner")
                    {
                        //spawn.gameObject.SetActive(false);
                    }
                }
            }




            //SpawnObjetoServidor();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_HandleHit()
    {
        Debug.Log("ENTROU AQUI, AGR VAI");

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.TryGetComponent(out NetworkObject netObj))
            {
                if (netObj.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteractObject(this);
                }
            }
        }

        
    }
    public void Interagir()
    {
        Debug.Log("Clicou para Interagir primeiramente");

        RPC_HandleHit();


        /*Debug.Log("Clicou para Interagir");



        if (hitInteract.collider)
        {
            hitInteract.collider.TryGetComponent(out IInteractable interactable);

            if (interactable != null)
            {
                interactable.OnInteractObject(this);
            }
        }
        else
        {
            Debug.Log("Não tem nada pra interagir.");
        }



        if (Object.HasInputAuthority)
        {
            
        }*/

    

        /*if (hitInteract.collider)
        {
            hitInteract.collider.TryGetComponent(out IInteractable interactable);

            if (interactable != null)
            {
                Rpc_Interagir();
            }
        }
        else
        {
            Debug.Log("Não tem nada pra interagir.");
        }*/
    }

    
    private void Rpc_Interagir()
    {
        Debug.Log("Foi para o rpc");

        // Raycast aqui no lado do servidor (usando a mesma lógica)
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 5, interactableLayer))
        {
            Debug.Log("ativou o raycast");
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                Debug.Log("entrou no if");
                interactable.OnInteractObject(this);
                
            }
        }
        else
        {
            Debug.Log("Nada pra interagir no lado do servidor.");
        }
    }

    public void OnPointerDown()
    {
        segurandoBotao = true;
    }

    bool segurandoBotao;
    public void OnPointerUp()
    {
        segurandoBotao = false;
        tempoPressionado = 0f;
    }

    private void Update()
    {
        if (segurandoBotao && tempoPressionado < limiteMaxParaPointerDownIniciar)
        {
            tempoPressionado += Time.deltaTime;
        }

        else if (tempoPressionado >= limiteMaxParaPointerDownIniciar)
        {
            if (Object.HasInputAuthority)
            {
                Debug.Log("Clicou para Interagir no POINT DOWN");

                if (hitInteract.collider)
                {
                    hitInteract.collider.TryGetComponent(out IInteractable interactable);

                    if (interactable != null)
                    {
                        interactable.OnInteractObject(this);
                    }
                }
                else
                {
                    Debug.Log("Não tem nada pra interagir.");
                }
            }
        }
    }


    /*public void SpawnObjetoServidor()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null)
        {
            var obj = Runner.Spawn(prefabGameManager, Vector3.zero, Quaternion.identity, PlayerRef.None);
            // Agora 'obj' está com StateAuthority no host
        }

    }*/
}
