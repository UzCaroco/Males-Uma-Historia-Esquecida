using Fusion;
using UnityEngine;

public class PickUpItem : NetworkBehaviour, IInteractable
{
    public ItemData itemData;
    [SerializeField] InteriorDoor interiorDoor;
    [SerializeField] UseItem useItem;
    [SerializeField] PickMosquetes pickMosquetes;
    public override void Spawned()
    {
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory.itemAtual == null)
        {
            playerInventory.RPC_AtivarSomDePickUp(); // Play pickup sound



            if (itemData != null)
            {
                if (interiorDoor != null)
                {
                    interiorDoor.RPC_MostrarOutline(); // Show outline if the item is a door
                }
                else if (useItem != null)
                {
                    useItem.RPC_MostrarOutline(); // Show outline if the item is a use item
                }
            }

            if (pickMosquetes != null)
            {
                Debug.Log("Pegou mosquete: ");
                pickMosquetes.RPC_PegouMosquete(); // Show outline if the item is a mosquete
            }


            Runner.Despawn(Object); // Despawns the network object
        }

        
    }
}
