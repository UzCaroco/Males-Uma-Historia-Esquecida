using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] Camera cam;
    Rigidbody rb;
    Animator ani;

    [SerializeField] GameObject playerModel;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Image mira;
    [SerializeField] LayerMask layerDaParede;
    Touch touchCamera, touchMovement;
    Vector3 vetor, vt;
    Vector2 posicaoInicialMovement;
    

    int idMovement = -1, idCamera = -1;
    [SerializeField] float porcentagem, velocity, sensibilidadeTouch;
    float divisaoCameraMovement, rotacaoX;
    float rotacaoHorizontal, rotacaoVertical;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ani = playerModel.GetComponent<Animator>();
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            cam.gameObject.SetActive(false); // Desativa a câmera dos outros players
            rb.isKinematic = true; // Impede que a física interfira na posição do player remoto
            return;
        }
    }
    void Start()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // Carrega a cena do outro mundo
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive); // Carrega a cena do outro mundo

        divisaoCameraMovement = porcentagem * Screen.width;

        // pega as rotações iniciais e armazena;
        rotacaoHorizontal = cameraPivot.transform.localEulerAngles.y;
        rotacaoVertical = cameraPivot.transform.localEulerAngles.x;
    }

    void Update()
    {
        if (!IsOwner) return;

        

            // Desenha uma linha/mira na tela para ver o que a câmera está vendo
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * 50, Color.red);

        if (Input.touchCount > 0)
        {
            foreach (Touch x in Input.touches)
            {
                switch (x.phase)
                {
                    case TouchPhase.Began:
                        {
                            // Camera
                            if (x.position.x > divisaoCameraMovement)
                            {
                                idCamera = x.fingerId;
                            }

                            // Movement
                            if (x.position.x <= divisaoCameraMovement)
                            {
                                idMovement = x.fingerId;
                                posicaoInicialMovement = x.position;
                            }
                        }
                        break;
                    case TouchPhase.Moved:
                        {
                            // Camera
                            if (x.fingerId == idCamera)
                            {
                                // Atualiza a rotação com base na movimentação do dedo tanto em X como em Y
                                rotacaoHorizontal += x.deltaPosition.x * sensibilidadeTouch;

                                rotacaoVertical -= x.deltaPosition.y * sensibilidadeTouch;
                                rotacaoVertical = Mathf.Clamp(rotacaoVertical, -40, 40); // Limita para a camera não da 360º na vertical
                            }

                            // Movement
                            if (x.fingerId == idMovement)
                            {
                                float moveX = x.position.x - posicaoInicialMovement.x;
                                float moveY = x.position.y - posicaoInicialMovement.y;

                                moveX = Mathf.Clamp(moveX, -180, 180);
                                moveY = Mathf.Clamp(moveY, -180, 180);

                                vetor = new Vector3(moveX, 0, moveY).normalized;

                                
                            }
                        }
                        break;
                    case TouchPhase.Ended:
                        {
                            // Camera
                            if (x.fingerId == idCamera)
                            {
                                idCamera = -1;
                            }

                            // Movement
                            if (x.fingerId == idMovement)
                            {
                                idMovement = -1;
                                vetor = Vector3.zero;
                            }
                        }
                        break;
                    case TouchPhase.Canceled:
                        {
                            // Camera
                            if (x.fingerId == idCamera)
                            {
                                idCamera = -1;
                            }

                            // Movement
                            if (x.fingerId == idMovement)
                            {
                                idMovement = -1;
                                vetor = Vector3.zero;
                            }
                        }
                        break;
                }


            }

            // Rotaciona o objeto pivot em relação a movimentação do dedo
            cameraPivot.localRotation = Quaternion.Euler(rotacaoVertical, rotacaoHorizontal, 0f);
            

        }
        cameraPivot.position = transform.position;


        if (vetor != Vector3.zero)
        {
            ani.SetBool("IsWalking", true);

            Vector3 direcaoComCamera = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * vetor;
            Quaternion novaRotacao = Quaternion.LookRotation(direcaoComCamera);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, novaRotacao, Time.deltaTime * 10f);
        }
        else
        {
            ani.SetBool("IsWalking", false);
        }



    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        // Faz a movimentação do player com base na direção ao qual a câmera está olhando e movimenta relacionando-o ao touch do dedo
        Vector3 direcao = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * vetor;
        vt = direcao * velocity * Time.fixedDeltaTime + rb.position;
        rb.MovePosition(vt);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            Scene cena2 = SceneManager.GetSceneByBuildIndex(1);
            SceneManager.MoveGameObjectToScene(gameObject, cena2); // Move o player para a cena do outro mundo

            SceneManager.UnloadSceneAsync(0); // Descarrega a cena do mundo inicial
        }

        
    }
    void PositionCamera()
    {
        
    }

    private void LateUpdate()
    {
        PositionCamera();
    }
}
