using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[RequireComponent(typeof(NetworkCharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator ani;
    [SerializeField] CharacterController caracter;
    [SerializeField] private NetworkCharacterController _cc;
    [SerializeField] NetworkTransform networkTransform;
    [SerializeField] NetworkRigidbody3D rbNetworked;
    [SerializeField] NetworkTRSP networkTRSP;

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

    // Par�metros para subir rampas
    [SerializeField] float slopeLimit = 45f; // �ngulo m�ximo de inclina��o que o personagem pode subir
    [SerializeField] float stepOffset = 0.3f; // Altura m�xima de um degrau que o personagem pode subir
    [SerializeField] float gravityMultiplier = 2f; // Multiplicador de gravidade para descida de rampas
    [SerializeField] LayerMask groundLayer; // Layer para detec��o do solo

    [Networked] private NetworkButtons NetworkButtons { get; set; }

    private void Awake()
    {
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
        Debug.Log(" " + Object.StateAuthority + Object.InputAuthority);

        if (HasStateAuthority)
        {
            Runner.SetPlayerObject(Object.InputAuthority, Object); // Set the player object for the input authority

            // get the NetworkCharacterController reference
            _cc = GetBehaviour<NetworkCharacterController>();
            Debug.Log("Foi atribuido?" + _cc.gameObject.name);
            networkTransform = GetBehaviour<NetworkTransform>();
            Debug.Log("Foi atribuido?" + networkTransform);
            rbNetworked = GetBehaviour<NetworkRigidbody3D>();
            Debug.Log("Foi atribuido?" + rbNetworked);
            networkTRSP = GetBehaviour<NetworkTRSP>();
            Debug.Log("Foi atribuido?" + networkTRSP);

            cam.gameObject.SetActive(Object.HasInputAuthority);
        }

    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);


    }

    void Start()
    {
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // Carrega a cena do outro mundo
        //SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive); // Carrega a cena do outro mundo

        divisaoCameraMovement = porcentagem * Screen.width;

        // pega as rota��es iniciais e armazena;
        rotacaoHorizontal = cameraPivot.transform.localEulerAngles.y;
        rotacaoVertical = cameraPivot.transform.localEulerAngles.x;

        posicaoPadraoJoystick = joystickPai.transform.position;

    }

    void Update()
    {
        if (!HasInputAuthority) return;



        // Desenha uma linha/mira na tela para ver o que a c�mera est� vendo
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
                                // Atualiza a rota��o com base na movimenta��o do dedo tanto em X como em Y
                                rotacaoHorizontal += x.deltaPosition.x * sensibilidadeTouch;

                                rotacaoVertical -= x.deltaPosition.y * sensibilidadeTouch;
                                rotacaoVertical = Mathf.Clamp(rotacaoVertical, -40, 40); // Limita para a camera n�o da 360� na vertical
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

            // Rotaciona o objeto pivot em rela��o a movimenta��o do dedo
            cameraPivot.localRotation = Quaternion.Euler(rotacaoVertical, rotacaoHorizontal, 0f);


        }
        //cameraPivot.position = transform.position;


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


        //cameraPivot.transform.position = Vector3.Lerp(transform.position, this.transform.position, Time.deltaTime * 15f);
        // playerModel.transform.position = Vector3.Lerp(transform.position, this.transform.position, Time.deltaTime * 15f);
        transform.parent.position = Vector3.Lerp(transform.position, this.transform.position, Time.deltaTime * 15f);


    }

    private void LateUpdate()
    {

    }

    // M�todo para verificar se est� em uma rampa
    private bool IsOnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, groundLayer))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            return angle > 0 && angle < slopeLimit;
        }
        return false;
    }

    // M�todo para obter a dire��o de movimento ajustada para rampas
    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f, groundLayer))
        {
            // Projeta o movimento na dire��o da rampa
            Vector3 slopeDirection = Vector3.ProjectOnPlane(direction, hit.normal).normalized;
            return slopeDirection;
        }
        return direction;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasInputAuthority) return;

        playerModel.transform.position = networkTRSP.transform.position; // Mant�m a altura do modelo do player igual � do NetworkTRSP

        // Faz a movimenta��o do player com base na dire��o ao qual a c�mera est� olhando
        Vector3 direcao = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * vetor;

        // Calcula a velocidade base
        float speed = _cc.maxSpeed;
        Vector3 moveDirection = direcao.normalized;

        // Verifica se est� em uma rampa e ajusta o movimento
        bool onSlope = IsOnSlope();
        if (onSlope && vetor != Vector3.zero)
        {
            // Ajusta a dire��o do movimento para seguir a inclina��o da rampa
            moveDirection = GetSlopeMoveDirection(direcao);

            // Adiciona um pequeno impulso vertical para ajudar a subir rampas
            // Este � o segredo para subir rampas com o CharacterController
            moveDirection.y = 0.5f;
        }

        // Calcula o vetor final de movimento
        vt = moveDirection * speed * Runner.DeltaTime;

        // Move o CharacterController com o vetor ajustado
        _cc.Move(vt);

        // Sincroniza a rota��o do objeto principal com a do modelo visual
        if (vetor != Vector3.zero && networkTransform != null)
        {
            transform.rotation = playerModel.transform.rotation;
        }
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
