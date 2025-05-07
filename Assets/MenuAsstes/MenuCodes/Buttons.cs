using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Buttons : MonoBehaviour
{
    public GameObject play, creditos, press;
    private void Start()
    {
      play.gameObject.gameObject.SetActive(false);
        creditos.gameObject.SetActive(false);   
        press.gameObject.SetActive(true);
    }
    public void StarButtons()
    {
        SceneManager.LoadScene("Fase 2");
    }
    public void CreditScene()
    {
        SceneManager.LoadScene("");
    }
    public void PressBUtton()
    {
        press.gameObject.SetActive(false);

    }
    public void Update()
    {
        if(press==false)
        {
            play.gameObject.gameObject.SetActive(true);
            creditos.gameObject.SetActive(true);

        }
    }
}
