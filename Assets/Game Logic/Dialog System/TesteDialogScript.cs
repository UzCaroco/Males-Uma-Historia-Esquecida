using cherrydev;
using Fusion;
using UnityEngine;

public class TesteDialogScript : NetworkBehaviour, IInteractable
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogNodeGraph;
    [SerializeField] AudioSource audioSource;

    NetworkBool dialogoInicial = false;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null && !dialogoInicial)
        {
            dialogoInicial = true;

            dialogBehaviour.StartDialog(dialogNodeGraph);
            audioSource.Play();

            playerInventory.RPC_AtivarMissoes(); // Chama o RPC para ativar as missões

            this.enabled = false; // Desabilita o script após iniciar o diálogo
        }

        
    }

    private void Start()
    {
        
    }
}
