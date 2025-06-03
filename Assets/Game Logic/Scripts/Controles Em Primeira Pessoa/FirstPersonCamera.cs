using UnityEngine.UI;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 10f;

    private float verticalRotation;
    private float horizontalRotation;





    int idCamera = -1;
    [SerializeField] float porcentagem, sensibilidadeTouch;
    float divisaoCameraMovement;
    float rotacaoHorizontal, rotacaoVertical;

    [SerializeField] Transform cameraPivot;
    [SerializeField] Image mira;

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

        transform.position = Target.position + Vector3.up * 1.6f;

        /*float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);*/
    }


}
