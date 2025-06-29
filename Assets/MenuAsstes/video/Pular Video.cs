using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PularVideo : MonoBehaviour
{
    [SerializeField] GameObject rawImage;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip cutsceneFase2;

    public void Pular()
    {
        // Desativa o canvas do vídeo
        rawImage.SetActive(false);

        PlayerSpawn playerSpawn = FindObjectOfType<PlayerSpawn>();
        if (playerSpawn != null)
        {
            playerSpawn.jaChegouEm90 = true; // Marca que já chegou em 90% do vídeo
        }
    }

    public void NovaCutscene(sbyte cutscene)
    {
        rawImage.SetActive(true); // Ativa o canvas do vídeo

        if (cutscene == 2)
        {
            Debug.Log("Nova cutscene iniciada: Fase 2");
            videoPlayer.clip = cutsceneFase2; // Define o novo vídeo
        }
        videoPlayer.Play(); // Inicia o novo vídeo
        Debug.Log("Novo vídeo iniciado: " + videoPlayer.clip.name);

    }

    private void Update()
    {
        if (rawImage == null)
        {
            rawImage = GameObject.Find("RawImage");
        }
        if (videoPlayer == null)
        {
            videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
        }
    }
}
