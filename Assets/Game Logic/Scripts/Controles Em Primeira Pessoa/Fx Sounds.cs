
using UnityEngine;

public class FxSounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickUp, drop, use, gaveta, bau, abrirPorta, fecharporta;

    public void PlayPickUpSound()
    {
        if (audioSource != null && pickUp != null)
        {
            audioSource.PlayOneShot(pickUp);
        }
    }
    public void PlayDropSound()
    {
        if (audioSource != null && drop != null)
        {
            audioSource.PlayOneShot(drop);
        }
    }
    public void PlayUseSound()
    {
        if (audioSource != null && use != null)
        {
            audioSource.PlayOneShot(use);
        }
    }
}
