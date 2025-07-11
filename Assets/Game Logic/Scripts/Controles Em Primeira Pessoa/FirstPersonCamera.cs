using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Video;
using Fusion;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 10f;

    private float verticalRotation;
    private float horizontalRotation;
    Camera cam;



    int idCamera = -1;
    [SerializeField] float porcentagem, sensibilidadeTouch;
    float divisaoCameraMovement;
    float rotacaoHorizontal, rotacaoVertical;

    [SerializeField] Transform cameraPivot;
    [SerializeField] Image mira;
    public Image slotItem;

    [SerializeField] float tempoPressionado = 0f, limiteMaxParaPointerDownIniciar = 5f;
    [SerializeField] LayerMask interactableLayer;



    [SerializeField] GameObject chestCode, livro, missoes, frase;
    [SerializeField] TextMeshProUGUI textoMissoes;

    [SerializeField] TMP_InputField inputFieldChestCode;
    [SerializeField] TextMeshProUGUI textoDoQuePrecisa;

    public string stringMissoes;

    public RaycastHit hitInteract => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    bool segurandoBotao;

    public bool estaAgachado = false;

    [SerializeField] VideoClip videoPrisao, videoArmas;

    void Start()
    {
        divisaoCameraMovement = porcentagem * Screen.width;

        // pega as rota��es iniciais e armazena;
        rotacaoHorizontal = cameraPivot.transform.localEulerAngles.y;
        rotacaoVertical = cameraPivot.transform.localEulerAngles.x;

    }
    private void Update()
    {
        // Desenha uma linha/mira na tela para ver o que a c�mera est� vendo
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.red);

        if (Input.touchCount > 0)
        {
            foreach (Touch x in Input.touches)
            {
                switch (x.phase)
                {
                    case UnityEngine.TouchPhase.Began:
                        {
                            // Camera
                            if (x.position.x > divisaoCameraMovement)
                            {
                                idCamera = x.fingerId;
                            }
                        }
                        break;
                    case UnityEngine.TouchPhase.Moved:
                        {
                            // Camera
                            if (x.fingerId == idCamera)
                            {
                                // Atualiza a rota��o com base na movimenta��o do dedo tanto em X como em Y
                                rotacaoHorizontal += x.deltaPosition.x * sensibilidadeTouch;

                                rotacaoVertical -= x.deltaPosition.y * sensibilidadeTouch;
                                rotacaoVertical = Mathf.Clamp(rotacaoVertical, -70, 70); // Limita para a camera n�o da 360� na vertical
                            }
                        }
                        break;
                    case UnityEngine.TouchPhase.Ended:
                        {
                            // Camera
                            if (x.fingerId == idCamera)
                            {
                                idCamera = -1;
                            }
                        }
                        break;
                    case UnityEngine.TouchPhase.Canceled:
                        {
                            // Camera
                            if (x.fingerId == idCamera)
                            {
                                idCamera = -1;
                            }

                        }
                        break;
                }
            }
            // Rotaciona o objeto pivot em rela��o a movimenta��o do dedo
            cameraPivot.localRotation = Quaternion.Euler(rotacaoVertical, rotacaoHorizontal, 0f);
        }
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        if (!estaAgachado)
        {
            transform.position = Target.position + Vector3.up * 1.6f;
        }
        else
        {
            transform.position = Target.position + Vector3.up * 0.75f; // Ajusta a altura da c�mera quando agachado
        }

        /*float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);*/
    }

    public void Interagir()
    {
        Debug.Log("Clicou para Interagir primeiramente");
        cam = Camera.main;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            Debug.Log("Raycast atingiu: " + hit.collider.name);

            if (hit.collider.TryGetComponent(out UseItem useItem))
            {
                if ((int)useItem._data.itemType == 12)
                {
                    if (Target.gameObject.TryGetComponent(out Inven inven))
                    {
                        if ((int)inven.itemAtual.itemType == 12)
                        {
                            PickMosquetes pickMosquetes = FindAnyObjectByType<PickMosquetes>();
                            if (pickMosquetes != null)
                            {
                                if (pickMosquetes.mosquetes < 8) //Se n�o pegou todos os mosquestes roda outra cutscene
                                {
                                    GameObject raw = FindAnyObjectByType<RawImage>(FindObjectsInactive.Include).gameObject; // Encontra a RawImage na cena
                                    Debug.Log("RawImage encontrada: " + raw.name);
                                    raw.gameObject.SetActive(true); // Ativa a RawImage
                                    var videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
                                    Debug.Log("VideoPlayer encontrado: " + videoPlayer.name);
                                    videoPlayer.clip = videoPrisao;
                                    videoPlayer.Play();
                                }
                                else //Se pegou todos os mosquetes roda cutscene da fase 3
                                {
                                    Debug.Log("CHEGOU AQUII pickMosquetes.mosquetes" + pickMosquetes.mosquetes);
                                    PlayerMovement playerQualquer = FindAnyObjectByType<PlayerMovement>();
                                    playerQualquer.RPC_DeathAndRespawnPlayer(new Vector3(-27.43f, 8.54f, 45.18f));
                                }
                            }
                            else
                            {
                                Debug.LogError("pickMosquetes n�o encontrado");
                            }
                        }
                    }
                }

                if (useItem._data.itemType == ItemType.Isqueiro)
                {
                    textoDoQuePrecisa.text = "Voc� precisa de um isqueiro para acender a lamparina.";
                    StartCoroutine(ResetarTexto());
                }
                else if (useItem._data.itemType == ItemType.EDeHabra)
                {
                    textoDoQuePrecisa.text = "Voc� precisa de um P� de cabra para abrir a gaveta";
                    StartCoroutine(ResetarTexto());
                }
                else if (useItem._data.itemType == ItemType.ChaveQuarto)
                {
                    textoDoQuePrecisa.text = "Voc� precisa de uma chave prateada para abrir a porta";
                    StartCoroutine(ResetarTexto());
                }
                else if (useItem._data.itemType == ItemType.ChaveSaida)
                {
                    textoDoQuePrecisa.text = "Voc� precisa de uma chave dourada para abrir o cadeado";
                    StartCoroutine(ResetarTexto());
                }
            }
            else //Aqui verifica se pegou todos os mosquestes, mas ainda n�o salvou os prisioneiros
            {
                if (hit.collider.TryGetComponent(out PickUpItem pickupItem))
                {
                    if ((int)pickupItem.itemData.itemType == 16)
                    {
                        PickMosquetes pickMosquetes = FindAnyObjectByType<PickMosquetes>();
                        if (pickMosquetes != null && pickMosquetes.mosquetes >= 7)
                        {
                            if (pickMosquetes.cadeadoPrisao != null)
                            {
                                Debug.Log("Pegou mosquete: " + pickMosquetes.mosquetes);
                                GameObject raw = FindAnyObjectByType<RawImage>(FindObjectsInactive.Include).gameObject; // Encontra a RawImage na cena
                                Debug.Log("RawImage encontrada: " + raw.name);
                                raw.gameObject.SetActive(true); // Ativa a RawImage
                                var videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
                                Debug.Log("VideoPlayer encontrado: " + videoPlayer.name);
                                videoPlayer.clip = videoArmas;
                                videoPlayer.Play();
                            }
                            else
                            {
                                PlayerMovement playerQualquer = FindAnyObjectByType<PlayerMovement>();
                                playerQualquer.RPC_DeathAndRespawnPlayer(new Vector3(-27.43f, 8.54f, 45.18f));
                            }
                        }
                    }
                }
            }
        }

        Target.gameObject.GetComponent<Inven>().RPC_HandleHit(cam.transform.position, cam.transform.forward);

        
    }

    public void OnPointerDown()
    {
        Debug.Log("Clicou para Interagir primeiramente");

        segurandoBotao = true;
    }

    
    public void OnPointerUp()
    {
        segurandoBotao = false;
        tempoPressionado = 0f;
    }


    public void Agachar()
    {
        if (Target.TryGetComponent(out PlayerMovement playerMovement))
        {
            if (!estaAgachado)
            {
                playerMovement.RPC_Agachar();
            }
            else
            {
                playerMovement.RPC_Levantar();
            }
        }
    }


    public void UpdateInteragir()
    {
        if (segurandoBotao && tempoPressionado < limiteMaxParaPointerDownIniciar)
        {
            tempoPressionado += Time.deltaTime;
        }

        else if (tempoPressionado >= limiteMaxParaPointerDownIniciar)
        {
            cam = Camera.main;
            Target.gameObject.GetComponent<Inven>().RPC_HandleHit(cam.transform.position, cam.transform.forward);
        }
    }




    public void AtivarChestCode()
    {
        if (chestCode != null)
        {
            if (chestCode.activeSelf)
            {
                chestCode.SetActive(false);
            }
            else
            {
                inputFieldChestCode.text = ""; // Limpa o campo de entrada antes de digitar o c�digo
                chestCode.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Chest code object is not assigned in the inspector.");
        }
    }

    public void FecharChestCode()
    {
        if (chestCode != null)
        {
            chestCode.SetActive(false);
        }
    }

    public void VerificarChestCode()
    {
        try
        {
            if (Target.TryGetComponent(out Inven inventario))
            {
                string resposta = inputFieldChestCode.text;
                inventario.RPC_VerificarChestCode(resposta);
            }
            else
            {
                Debug.LogError("Inven component not found on Target.");
            }

            if (chestCode != null)
            {
                chestCode.SetActive(false);
            }
        }
        catch (Exception)
        {
            Debug.LogError("DEU ERRO");
        }
    }





    public void AtivarLivro()
    {
        if (livro != null)
        {
            if (livro.activeSelf)
            {
                livro.SetActive(false);
            }
            else
            {
                livro.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Chest code object is not assigned in the inspector.");
        }
    }

    

    public void FecharLivro()
    {
        if (livro != null)
        {
            livro.SetActive(false);
        }
    }


    public void AtivarFrase()
    {
        if (frase != null)
        {
            if (frase.activeSelf)
            {
                frase.SetActive(false);
            }
            else
            {
                frase.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Chest code object is not assigned in the inspector.");
        }
    }

    public void FecharFrase()
    {
        if (frase != null)
        {
            frase.SetActive(false);
        }
    }



    public void AtivarMissoes(string missao)
    {
        if (missoes != null)
        {
            textoMissoes.text = missao; // Atualiza o texto das miss�es com a nova miss�o
            missoes.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Chest code object is not assigned in the inspector.");
        }
    }


    IEnumerator ResetarTexto()
    {
        yield return new WaitForSeconds(5f);
        textoDoQuePrecisa.text = "";
    }
}
