using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Buttons : MonoBehaviour
{
    public GameObject play, sair, press;
    private void Start()
    {
        play.gameObject.gameObject.SetActive(false);
        sair.gameObject.SetActive(false);   
        press.gameObject.SetActive(true);
    }
    public void StartButton()
    {
        SceneManager.LoadScene("Meu Host");
    }
    public void CreditButton()
    {

    }
    public void PressBUtton()
    {
        Touch touch=Input.GetTouch(0);
        press.gameObject.SetActive(false);
        play.gameObject.gameObject.SetActive(true);
        sair.gameObject.SetActive(true);

    }
    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                PressBUtton();
            }
        }
    
    }
}
