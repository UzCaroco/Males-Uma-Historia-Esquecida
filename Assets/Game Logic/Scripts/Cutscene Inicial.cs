using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Fusion;
public class CutsceneInicial : NetworkBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Canvas canvasVideo;
    [SerializeField] RawImage videoTexture;

    bool jaChegouEm90 = false;

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            videoPlayer.enabled = true;
            videoPlayer.Play();

            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            // Desativa o canvas e o vídeo pros outros jogadores
            canvasVideo.enabled = false;
            videoPlayer.gameObject.SetActive(false);
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (HasInputAuthority)
        {
            canvasVideo.enabled = false;
        }
    }


    void Update()
    {
        if (!HasInputAuthority) return;

        if (videoPlayer.isPlaying && !jaChegouEm90)
        {
            double progresso = videoPlayer.time / videoPlayer.length;

            if (progresso >= 0.92)
            {
                jaChegouEm90 = true;
                Debug.Log("Chegou em 92% do vídeo!");

                videoTexture.gameObject.SetActive(false); // Remove a textura do vídeo

            }
        }
    }
}
