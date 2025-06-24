using UnityEngine.UI;
using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private NetworkCharacterController _controller;
    public Camera Cam;
    public float PlayerSpeed = 2f;
    [SerializeField] ParticleSystem fireGun;

    Animator ani;
    int walkingHash = Animator.StringToHash("IsWalking");
    int crouchHash = Animator.StringToHash("IsCrouching");

    [SerializeField] LayerMask mesaLayer;

    bool sobMesa => Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 3f, mesaLayer);

    CharacterController cc;
    private void Awake()
    {
        _controller = GetComponent<NetworkCharacterController>();
        ani = GetComponent<Animator>();
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Cam = Camera.main;
            Cam.GetComponent<FirstPersonCamera>().Target = transform;

            cc = GetComponent<CharacterController>();

            joystickPai = GameObject.Find("Joy Pai").GetComponent<Image>();
            joystickFilho = GameObject.Find("Joy Filho").GetComponent<Image>();
        }
    }










    int idMovement = -1;
    [SerializeField] float porcentagem, velocity;
    float divisaoCameraMovement;

    [SerializeField] Image joystickPai, joystickFilho;
    Vector3 vetor, vt;
    Vector2 posicaoInicialMovement, posicaoPadraoJoystick;

    private void Start()
    {
        divisaoCameraMovement = porcentagem * Screen.width;
        posicaoPadraoJoystick = joystickPai.transform.position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fireGun.Play();
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch x in Input.touches)
            {
                switch (x.phase)
                {
                    case UnityEngine.TouchPhase.Began:
                        {
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

                                if (ani.GetBool(walkingHash) == false)
                                {
                                    ani.SetBool(walkingHash, true);
                                }
                            }
                        }
                        break;
                    case UnityEngine.TouchPhase.Ended:
                        {
                            // Movement
                            if (x.fingerId == idMovement)
                            {
                                idMovement = -1;
                                vetor = Vector3.zero;

                                joystickPai.transform.position = posicaoPadraoJoystick;
                                joystickFilho.transform.position = posicaoPadraoJoystick;

                                if (ani.GetBool(walkingHash) == true)
                                {
                                    ani.SetBool(walkingHash, false);
                                }
                            }
                        }
                        break;
                    case UnityEngine.TouchPhase.Canceled:
                        {
                            // Movement
                            if (x.fingerId == idMovement)
                            {
                                idMovement = -1;
                                vetor = Vector3.zero;

                                joystickPai.transform.position = posicaoPadraoJoystick;
                                joystickFilho.transform.position = posicaoPadraoJoystick;

                                if (ani.GetBool(walkingHash) == true)
                                {
                                    ani.SetBool(walkingHash, false);
                                }
                            }
                        }
                        break;
                }


            }



        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Agachar()
    {
        if (ani.GetBool(crouchHash) == false)
        {
            ani.SetBool(crouchHash, true);
            cc.height = 0.75f;
            cc.center = new Vector3(0, 0.47f, 0);

            Cam.GetComponent<FirstPersonCamera>().estaAgachado = true;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Levantar()
    {
        if (ani.GetBool(crouchHash) == true && !sobMesa)
        {
            ani.SetBool(crouchHash, false);
            cc.height = 1.75f;
            cc.center = new Vector3(0, 0.93f, 0);
            Cam.GetComponent<FirstPersonCamera>().estaAgachado = false;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DeathAndRespawnPlayer(Vector3 posicaoDeRespawn)
    {
        _controller.Teleport(posicaoDeRespawn);
    }





    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority
        
        Quaternion cameraRotationY = Quaternion.Euler(0, Cam.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * vetor * Runner.DeltaTime * PlayerSpeed;

        

        _controller.Move(move);



        if (move == Vector3.zero)
        {
            Vector3 cameraForward = Cam.transform.forward;
            cameraForward.y = 0; // Zera o Y pra não inclinar

            gameObject.transform.forward = cameraForward;
        }
        else
        {
            gameObject.transform.forward = move;
        }
    }

}
