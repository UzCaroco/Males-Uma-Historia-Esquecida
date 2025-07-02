using System.Collections;
using TMPro;
using UnityEngine;

public class CronometroFase2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoTempo;
    [SerializeField] int tempoLimite = 600; // Tempo limite em segundos (10 minutos)
    void Start()
    {
        StartCoroutine(Temporizador()); // Inicia o temporizador
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Temporizador()
    {
        while (tempoLimite > 0)
        {
            yield return new WaitForSeconds(1);
            tempoLimite--;
            AtualizarTexto();
        }

        if (tempoLimite <= 0)
        {
            Debug.Log("Tempo esgotado!");
        }
    }

    void AtualizarTexto()
    {
        int minutos = tempoLimite / 60;
        int segundos = tempoLimite % 60;
        textoTempo.text = string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }
}
