using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TriggerVideofase3 : MonoBehaviour
{
    [SerializeField] VideoClip cutscene3; // V�deo que ser� reproduzido quando o player entrar no trigger
    [SerializeField] GameObject rawImage; // RawImage que ser� ativada
    [SerializeField] VideoPlayer videoPlayer; // VideoPlayer que reproduzir� o v�deo
    bool isVideoPlaying = false; // Flag para verificar se o v�deo est� sendo reproduzido
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isVideoPlaying)
        {
            isVideoPlaying = true; // Define a flag como verdadeira para evitar m�ltiplas ativa��es
            rawImage.SetActive(true);
            videoPlayer.clip = cutscene3;
            videoPlayer.Play();
        }
    }
}
