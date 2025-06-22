using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

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



    [SerializeField] GameObject chestCode, livro, missoes;
    [SerializeField] TextMeshProUGUI textoMissoes;

    [SerializeField] TMP_InputField inputFieldChestCode;

    public string stringMissoes;

    public RaycastHit hitInteract => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5, interactableLayer) ? hit : default;
    bool segurandoBotao;

    void Start()
    {
        divisaoCameraMovement = porcentagem * Screen.width;

        // pega as rotações iniciais e armazena;
        rotacaoHorizontal = cameraPivot.transform.localEulerAngles.y;
        rotacaoVertical = cameraPivot.transform.localEulerAngles.x;

    }
    private void Update()
    {
        // Desenha uma linha/mira na tela para ver o que a câmera está vendo
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
                                // Atualiza a rotação com base na movimentação do dedo tanto em X como em Y
                                rotacaoHorizontal += x.deltaPosition.x * sensibilidadeTouch;

                                rotacaoVertical -= x.deltaPosition.y * sensibilidadeTouch;
                                rotacaoVertical = Mathf.Clamp(rotacaoVertical, -70, 70); // Limita para a camera não da 360º na vertical
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
            // Rotaciona o objeto pivot em relação a movimentação do dedo
            cameraPivot.localRotation = Quaternion.Euler(rotacaoVertical, rotacaoHorizontal, 0f);
        }
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.position + Vector3.up * 1.6f;

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
            playerMovement.RPC_Agachar();
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
                inputFieldChestCode.text = ""; // Limpa o campo de entrada antes de digitar o código
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
        if (chestCode != null)
        {
            livro.SetActive(false);
        }
    }






    public void AtivarMissoes()
    {
        if (missoes != null)
        {
            if (missoes.activeSelf)
            {
                missoes.SetActive(false);
            }
            else
            {
                missoes.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Chest code object is not assigned in the inspector.");
        }
    }
}
