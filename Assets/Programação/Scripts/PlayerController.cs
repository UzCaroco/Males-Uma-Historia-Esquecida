using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera cam;
    Rigidbody rb;

    [SerializeField] Transform cameraPivot;
    Touch touchCamera, touchMovement;
    Vector3 vetor, vt;
    Vector2 posicaoInicialMovement;

    int idMovement = -1, idCamera = -1;
    [SerializeField] float porcentagem, velocity, sensibilidadeTouch;
    float divisaoCameraMovement, rotacaoX;
    float rotacaoHorizontal, rotacaoVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        divisaoCameraMovement = porcentagem * Screen.width;

        // pega as rotações iniciais e armazena;
        rotacaoHorizontal = cameraPivot.transform.localEulerAngles.y;
        rotacaoVertical = cameraPivot.transform.localEulerAngles.x;
    }

    void Update()
    {

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







                                /*float mouseX = x.deltaPosition.x * sensibilidadeTouch;
                                float mouseY = x.deltaPosition.y * sensibilidadeTouch;

                                rotacaoX -= mouseY;
                                rotacaoX = Mathf.Clamp(rotacaoX, -80, 80);

                                cam.transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f); 
                                transform.Rotate(Vector3.up * mouseX);*/
                            }

                            // Movement
                            if (x.fingerId == idMovement)
                            {
                                float moveX = x.position.x - posicaoInicialMovement.x;
                                float moveY = x.position.y - posicaoInicialMovement.y;

                                moveX = Mathf.Clamp(moveX, -180, 180);
                                moveY = Mathf.Clamp(moveY, -180, 180);

                                vetor = new Vector3(moveX, 0, moveY).normalized;

                                if (idCamera != -1)
                                {
                                    transform.localRotation = Quaternion.Euler(0, moveX, 0);
                                }
                                else
                                {
                                    transform.Rotate(0, rotacaoHorizontal, 0);
                                    rotacaoHorizontal = 0f;
                                    cameraPivot.localRotation = Quaternion.Euler(0, rotacaoHorizontal, 0);
                                }
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
            cameraPivot.position = transform.position;
        }





    }

    private void FixedUpdate()
    {
        // Faz a movimentação do player com base na direção ao qual a câmera está olhando e movimenta relacionando-o ao touch do dedo
        Vector3 direcao = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * vetor;
        vt = direcao * velocity * Time.fixedDeltaTime + rb.position;
        rb.MovePosition(vt);
    }
}
