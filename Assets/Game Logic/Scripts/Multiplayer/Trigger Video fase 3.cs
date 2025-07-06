using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Video;

public class TriggerVideofase3 : MonoBehaviour
{
    [SerializeField] VideoClip cutscene3; // Vídeo que será reproduzido quando o player entrar no trigger
    [SerializeField] GameObject rawImage; // RawImage que será ativada
    [SerializeField] VideoPlayer videoPlayer; // VideoPlayer que reproduzirá o vídeo
    bool isVideoPlaying = false; // Flag para verificar se o vídeo está sendo reproduzido
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isVideoPlaying)
        {
            isVideoPlaying = true; // Define a flag como verdadeira para evitar múltiplas ativações
            rawImage.SetActive(true);
            videoPlayer.clip = cutscene3;
            videoPlayer.Play();


            SpawnarNovaFase();


        }
    }

    private void SpawnarNovaFase()
    {
        SpawnNewPhase spawnPhase = FindAnyObjectByType<SpawnNewPhase>();
        if (spawnPhase != null)
        {
            spawnPhase.VerificarPlayerComAutoridade(); // Chama o RPC para spawnar a nova fase
            Debug.Log("Nova fase spawnada com sucesso.");
        }
    }
}
