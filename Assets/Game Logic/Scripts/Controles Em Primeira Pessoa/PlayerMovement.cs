using UnityEngine.UI;
using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private NetworkCharacterController _controller;
    public Camera Cam;
    public float PlayerSpeed = 2f;
    [SerializeField] ParticleSystem fireGun;


    private void Awake()
    {
        _controller = GetComponent<NetworkCharacterController>();
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Cam = Camera.main;
            Cam.GetComponent<FirstPersonCamera>().Target = transform;


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
                            }
                        }
                        break;
                }


            }

        }
    }










    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority
        
        Quaternion cameraRotationY = Quaternion.Euler(0, Cam.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * vetor * Runner.DeltaTime * PlayerSpeed;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }

}
