using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TriggerSpawnNewPhase : MonoBehaviour
{
    [SerializeField] VideoClip cutscene2; // Vídeo que será reproduzido quando o player entrar no trigger
    [SerializeField] GameObject rawImage; // RawImage que será ativada
    [SerializeField] VideoPlayer videoPlayer; // VideoPlayer que reproduzirá o vídeo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var localId = other.GetComponent<PlayerLocalIdentifier>();
            if (localId != null && localId.isLocalPlayer)
            {
                rawImage.SetActive(true);
                videoPlayer.clip = cutscene2;
                videoPlayer.Play();
            }
        }
    }
}
