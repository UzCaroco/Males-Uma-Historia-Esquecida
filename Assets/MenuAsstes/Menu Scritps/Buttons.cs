using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Buttons : MonoBehaviour
{
    public Button play, sair, creditos;
    public GameObject press, creditosPanel;

    public TextMeshProUGUI pressButton;

    float alpha = 0;
    bool alphaBool = false;
    private void Start()
    {
        StartCoroutine(AlphaAlternate());

        play.gameObject.gameObject.SetActive(false);
        sair.gameObject.SetActive(false);   
        creditos.gameObject.SetActive(false);
        press.gameObject.SetActive(true);
    }
    public void StartButton()
    {
        SceneManager.LoadScene("Meu Host");
    }
    public void PressBUtton()
    {
        Touch touch=Input.GetTouch(0);
        press.gameObject.SetActive(false);
        play.gameObject.gameObject.SetActive(true);
        sair.gameObject.SetActive(true);
        creditos.gameObject.SetActive(true);
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
        

        if (!alphaBool)
        {
            alpha += Time.deltaTime * 0.5f;
            pressButton.color = new Color(1, 1, 1, alpha);
        }

        if (alphaBool)
        {
            alpha -= Time.deltaTime * 0.5f;
            pressButton.color = new Color(1, 1, 1, alpha);
        }
    }

    IEnumerator AlphaAlternate()
    {
        yield return new WaitForSeconds(1f);
        if (alphaBool)
        {
            alphaBool = false;
        }
        else
        {
            alphaBool = true;
        }

        StartCoroutine(AlphaAlternate());
    }

    public void CreditsButton()
    {
        if (creditosPanel.activeSelf)
        {
            creditosPanel.SetActive(false);
        }
        else
        {
            creditosPanel.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void PlayPointerEnter()
    {
        play.image.color = new Color(1, 1, 1, 1f);
    }
    public void PlayPointerExit()
    {
        play.image.color = new Color(1, 1, 1, 0f);
    }
    public void SairPointerEnter()
    {
        sair.image.color = new Color(1, 1, 1, 1f);
    }
    public void SairPointerExit()
    {
        sair.image.color = new Color(1, 1, 1, 0f);
    }
    public void CreditPointerEnter()
    {
        creditos.image.color = new Color(1, 1, 1, 1f);
    }
    public void CreditPointerExit()
    {
        creditos.image.color = new Color(1, 1, 1, 0f);
    }

}
