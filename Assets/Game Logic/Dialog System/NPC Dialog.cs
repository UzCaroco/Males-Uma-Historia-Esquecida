using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NPCDialog : NetworkBehaviour, IInteractable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] itemDialogues; // Array of dialogues for each item

    sbyte index = 0; // Index to track the current dialogue

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (index < itemDialogues.Length)
        {
            audioSource.clip = itemDialogues[index]; // Set the audio clip to the initial dialogue
            audioSource.Play(); // Play the initial dialogue
            index++; // Increment the index for the next dialogue
        }
        else
        {
            // If all dialogues have been played, you can reset the index or handle it as needed
            Debug.Log("All dialogues have been played.");
            index = 0; // Reset index if you want to loop through dialogues again
            audioSource.clip = itemDialogues[index]; // Set the audio clip to the initial dialogue
            audioSource.Play(); // Play the initial dialogue
        }
    }
}
