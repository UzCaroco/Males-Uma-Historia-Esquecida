using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FusionDemo {
  /// <summary>
  /// A simple networked player movement class for host/server mode.
  /// </summary>
  //[RequireComponent(typeof(NetworkCharacterController))]
  public class PlayerMovementHost : NetworkBehaviour {
        [SerializeField] Camera cam;
        [SerializeField] Rigidbody rb;
        [SerializeField] Animator ani;
        [SerializeField] private NetworkCharacterController _cc;

        [SerializeField] GameObject playerModel;
        [SerializeField] Transform cameraPivot;
        [SerializeField] Image mira, joystickPai, joystickFilho;
        [SerializeField] LayerMask layerDaParede;
        Touch touchCamera, touchMovement;
        Vector3 vetor, vt;
        Vector2 posicaoInicialMovement, posicaoPadraoJoystick;


        int idMovement = -1, idCamera = -1;
        [SerializeField] float porcentagem, velocity, sensibilidadeTouch;
        float divisaoCameraMovement, rotacaoX;
        float rotacaoHorizontal, rotacaoVertical;



        [Networked] private NetworkButtons NetworkButtons { get; set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            ani = playerModel.GetComponent<Animator>();
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
        public override void Spawned()
        {
            base.Spawned();

            Debug.Log(" " + Object.StateAuthority + Object.InputAuthority);

            if (HasInputAuthority)
            {
                // get the NetworkCharacterController reference
                _cc = GetBehaviour<NetworkCharacterController>();
                Debug.Log("Foi atribuido?" + _cc);
            }

            if (!HasInputAuthority) //se não for o dono
            {
                Debug.Log("Sem autoridade de input nesse player! " + Object.InputAuthority);

                cam.gameObject.SetActive(false); // Desativa a câmera dos outros players

                //rb.isKinematic = true; // Impede que a física interfira na posição do player remoto
                return;
            }
        }
        void Start()
        {
            //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // Carrega a cena do outro mundo
            //SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive); // Carrega a cena do outro mundo

            divisaoCameraMovement = porcentagem * Screen.width;

            // pega as rotações iniciais e armazena;
            rotacaoHorizontal = cameraPivot.transform.localEulerAngles.y;
            rotacaoVertical = cameraPivot.transform.localEulerAngles.x;

            posicaoPadraoJoystick = joystickPai.transform.position;
        }

        void Update()
        {
            if (!HasInputAuthority) return;


            // Desenha uma linha/mira na tela para ver o que a câmera está vendo
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * 50, Color.red);
            
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

                                // Movement
                                if (x.position.x <= divisaoCameraMovement)
                                {
                                    idMovement = x.fingerId;
                                    posicaoInicialMovement = x.position;
                                    joystickPai.transform.position = x.position;
                                    joystickFilho.transform.position = x.position;
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
                                    rotacaoVertical = Mathf.Clamp(rotacaoVertical, -40, 40); // Limita para a camera não da 360º na vertical
                                }

                                // Movement
                                if (x.fingerId == idMovement)
                                {
                                    Vector2 direcao = x.position - posicaoInicialMovement;

                                    if (direcao.magnitude > 170f)
                                    {
                                        direcao = direcao.normalized * 170f;
                                    }
                                    joystickFilho.transform.position = posicaoInicialMovement + direcao;

                                    float moveX = x.position.x - posicaoInicialMovement.x;
                                    float moveY = x.position.y - posicaoInicialMovement.y;

                                    moveX = Mathf.Clamp(moveX, -180, 180);
                                    moveY = Mathf.Clamp(moveY, -180, 180);

                                    vetor = new Vector3(moveX, 0, moveY).normalized;


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

                                // Movement
                                if (x.fingerId == idMovement)
                                {
                                    idMovement = -1;
                                    vetor = Vector3.zero;

                                    joystickPai.transform.position = posicaoPadraoJoystick;
                                    joystickFilho.transform.position = posicaoPadraoJoystick;
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

                                // Movement
                                if (x.fingerId == idMovement)
                                {
                                    idMovement = -1;
                                    vetor = Vector3.zero;

                                    joystickPai.transform.position = posicaoPadraoJoystick;
                                    joystickFilho.transform.position = posicaoPadraoJoystick;
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

        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (!HasInputAuthority) return;

            Vector2 moveInput = Vector2.zero;
            Vector2 moveDelta = moveInput * 7 * Runner.DeltaTime;
            Vector3 vector3 = new Vector3(moveDelta.x, 0, moveDelta.y);
            Debug.Log(vector3);

            // Faz a movimentação do player com base na direção ao qual a câmera está olhando e movimenta relacionando-o ao touch do dedo
            Vector3 direcao = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * vetor;
            vt = direcao * velocity * Runner.DeltaTime;
            _cc.Move(vt);

            // If we received input from the input authority
            // The NetworkObject input authority AND the server/host will have the inputs
            /*if (GetInput<DemoNetworkInput>(out var input))
            {
                var dir = default(Vector3);

                // Handle horizontal input
                if (input.IsDown(DemoNetworkInput.BUTTON_RIGHT))
                {
                    dir += Vector3.right;
                }
                else if (input.IsDown(DemoNetworkInput.BUTTON_LEFT))
                {
                    dir += Vector3.left;
                }

                // Handle vertical input
                if (input.IsDown(DemoNetworkInput.BUTTON_FORWARD))
                {
                    dir += Vector3.forward;
                }
                else if (input.IsDown(DemoNetworkInput.BUTTON_BACKWARD))
                {
                    dir += Vector3.back;
                }

                // Move with the direction calculated
                _cc.Move(dir.normalized);

                // Store the current buttons to use them on the next FUN (FixedUpdateNetwork) call
                NetworkButtons = input.Buttons;
            }*/


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
    }
}