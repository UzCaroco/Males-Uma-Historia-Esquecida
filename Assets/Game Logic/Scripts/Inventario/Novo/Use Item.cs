using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UseItem : NetworkBehaviour, IInteractable
{
    [SerializeField] ItemData _data;

    [SerializeField] Sprite slotVazio;

    [SerializeField] DoorPadlock doorPadlock;
    [SerializeField] NetworkObject lamparinaPickUp;


    [SerializeField] NetworkBool paes, tapetes, castanhas;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        if (playerInventory != null)
        {
            Debug.Log("Inventário do jogador não é nulo" + playerInventory.Object);
            if (playerInventory.itemAtualID != -1)
            {
                Debug.Log("tem item");
                if (playerInventory.itemAtualID == (int)_data.itemType)
                {
                    Debug.Log("item é igual");



                    if ((int)_data.itemType == 3) //Se for a chave de saida
                    {
                        transform.Rotate(Vector3.up, 90f); // uso do item

                        if (doorPadlock != null) // Se o padlock não for nulo
                        {
                            doorPadlock.RPC_OpenLock(); // Chama o RPC para abrir a fechadura
                        }
                    }

                    if ((int)_data.itemType == 2) //Se for a caixa de fosforos
                    {
                        RPC_AcenderLamparina(); // Chama o RPC para acender a lamparina
                    }

                    if ((int)_data.itemType == 4) // Se for a lanterna
                    {
                        transform.Rotate(Vector3.forward * -90f); // abrindo porta
                    }

                    if ((int)_data.itemType == 7) // Se for a lanterna
                    {
                        tapetes = true;
                        Debug.Log("Pegou o tapete:");
                    }
                    if ((int)_data.itemType == 8) // Se for o pão
                    {
                        paes = true;
                        Debug.Log("Pegou o pão:");
                    }
                    if ((int)_data.itemType == 9) // Se for a castanha
                    {
                        castanhas = true;
                        Debug.Log("Pegou a castanha:");
                    }



                    playerInventory.RPC_ResetValues();

                }
                else
                {
                    Debug.Log("item é diferente");
                }
            }
            else
            {
                Debug.Log("Não tem item");
            }
        }
        else
        {
            Debug.Log("Inventário do jogador é nulo");
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AcenderLamparina()
    {
        Debug.Log("Acendendo a lamparina");
        Runner.Spawn(lamparinaPickUp, transform.position, transform.rotation, inputAuthority: Runner.LocalPlayer); // Spawns a lamparina na posiçao do item
        Runner.Despawn(Object); // Despawns o item
        
    }
}
