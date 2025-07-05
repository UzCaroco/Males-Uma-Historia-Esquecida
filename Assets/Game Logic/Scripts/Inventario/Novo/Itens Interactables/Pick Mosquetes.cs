using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PickMosquetes : NetworkBehaviour
{
    [Networked][SerializeField] public int mosquetes { get; set; } = 0;

    [SerializeField] NetworkObject cadeadoPrisao;
    [SerializeField] VideoClip videoArmas, cutscene3;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_PegouMosquete()
    {
        mosquetes++;
        Debug.Log("Pegou mosquete: " + mosquetes);

        if (cadeadoPrisao == null)
        {
            if (mosquetes == 8)
            {
                var raws = FindObjectsByType<RawImage>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                foreach (var rawIma in raws)
                {
                    Debug.Log("RawImage encontrada: " + rawIma.name);
                    rawIma.gameObject.SetActive(true);
                }

                var videos = FindObjectsByType<VideoPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                foreach (var vid in videos)
                {
                    Debug.Log("VideoPlayer encontrado: " + vid.name);
                    vid.clip = cutscene3;
                    vid.Play();
                }

                //GameObject raw = FindAnyObjectByType<RawImage>(FindObjectsInactive.Include).gameObject; // Encontra a RawImage na cena
                //Debug.Log("RawImage encontrada: " + raw.name);
                //raw.gameObject.SetActive(true); // Ativa a RawImage

                //var videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
                //Debug.Log("VideoPlayer encontrado: " + videoPlayer.name);
                //videoPlayer.clip = cutscene3;
                //videoPlayer.Play();
            }
            
        }
        
        else
        {
            if (mosquetes == 8)
            {
                foreach (PlayerLocalIdentifier player in FindObjectsOfType<PlayerLocalIdentifier>())
                {
                    if (player != null && player.isLocalPlayer)
                    {
                        GameObject raw = FindAnyObjectByType<RawImage>(FindObjectsInactive.Include).gameObject; // Encontra a RawImage na cena
                        Debug.Log("RawImage encontrada: " + raw.name);
                        raw.gameObject.SetActive(true); // Ativa a RawImage
                        var videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
                        Debug.Log("VideoPlayer encontrado: " + videoPlayer.name);
                        videoPlayer.clip = videoArmas;
                        videoPlayer.Play();
                    }
                }
            }
        }
    }
}
