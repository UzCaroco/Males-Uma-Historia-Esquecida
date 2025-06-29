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
        // Desativa o canvas do v�deo
        rawImage.SetActive(false);

        PlayerSpawn playerSpawn = FindObjectOfType<PlayerSpawn>();
        if (playerSpawn != null)
        {
            playerSpawn.jaChegouEm90 = true; // Marca que j� chegou em 90% do v�deo
        }
    }

    public void NovaCutscene(sbyte cutscene)
    {
        rawImage.SetActive(true); // Ativa o canvas do v�deo

        if (cutscene == 2)
        {
            Debug.Log("Nova cutscene iniciada: Fase 2");
            videoPlayer.clip = cutsceneFase2; // Define o novo v�deo
        }
        videoPlayer.Play(); // Inicia o novo v�deo
        Debug.Log("Novo v�deo iniciado: " + videoPlayer.clip.name);

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
