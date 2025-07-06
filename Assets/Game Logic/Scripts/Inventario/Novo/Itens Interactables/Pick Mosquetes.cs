using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PickMosquetes : NetworkBehaviour
{
    [Networked][SerializeField] public int mosquetes { get; set; } = 0;

    public NetworkObject cadeadoPrisao;
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

                PlayerMovement playerQualquer = FindAnyObjectByType<PlayerMovement>();
                playerQualquer.RPC_DeathAndRespawnPlayer(new Vector3(-27.43f, 8.54f, 45.18f));

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
                //foreach (PlayerLocalIdentifier player in FindObjectsOfType<PlayerLocalIdentifier>())
                //{
                //    if (player != null && player.isLocalPlayer)
                //    {
                //        GameObject raw = FindAnyObjectByType<RawImage>(FindObjectsInactive.Include).gameObject; // Encontra a RawImage na cena
                //        Debug.Log("RawImage encontrada: " + raw.name);
                //        raw.gameObject.SetActive(true); // Ativa a RawImage
                //        var videoPlayer = GameObject.Find("Video").GetComponent<VideoPlayer>();
                //        Debug.Log("VideoPlayer encontrado: " + videoPlayer.name);
                //        videoPlayer.clip = videoArmas;
                //        videoPlayer.Play();
                //    }
                //}
            }
        }
    }
}
