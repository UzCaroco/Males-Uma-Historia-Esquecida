using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TriggerSpawnNewPhase : MonoBehaviour
{
    [SerializeField] VideoClip cutscene2; // V�deo que ser� reproduzido quando o player entrar no trigger
    [SerializeField] GameObject rawImage; // RawImage que ser� ativada
    [SerializeField] VideoPlayer videoPlayer; // VideoPlayer que reproduzir� o v�deo
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
