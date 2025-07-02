using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UseItem : NetworkBehaviour, IInteractable
{
    [SerializeField] public ItemData _data;

    [SerializeField] Sprite slotVazio;

    [SerializeField] DoorPadlock doorPadlock;
    [SerializeField] NetworkObject lamparinaPickUp;
    [SerializeField] InteriorDoor portaCamara;
    [SerializeField] DoorPrision doorPrision;
    [SerializeField] GameObject ferroCadeado;


    [SerializeField] NetworkBool paes, tapetes, castanhas;


    [SerializeField] VideoClip videoClipPrisao;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        Debug.Log("Esta ativado?: " + this.enabled);
        if (!this.enabled) return; // Verifica se o script está habilitado, se não estiver, não faz nada

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
                        GetComponent<AudioSource>().Play(); // Toca o som do uso do item

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

                    if ((int)_data.itemType == 6) //Se for igual ao É de Habra
                    {
                        RPC_AbrirGaveta();
                    }



                    //Se for qualquer chave da camara
                    if ((int)_data.itemType == 10 || (int)_data.itemType == 11 || (int)_data.itemType == 12 || (int)_data.itemType == 13 || (int)_data.itemType == 14 || (int)_data.itemType == 15)
                    {
                        if (portaCamara != null) // Se a porta da câmara não for nula
                        {
                            portaCamara.destravado = true; // Marca a porta como destravada
                            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible; // Ativa o outline
                            this.enabled = false; // Desabilita o script para não permitir mais interações com este item
                        }
                    }

                    //Se for a chave do cadeado da prisao
                    if ((int)_data.itemType == 12)
                    {
                        if (doorPrision != null) // Se a porta da prisão não for nula
                        {
                            doorPrision.RPC_OpenDoorPrision(); //Abre a porta da prisão e salva licutan
                            playerInventory.RPC_MissaoConcluidaTeleportePlayer();

                            ferroCadeado.SetActive(false); // Desativa a corrente
                            Runner.Despawn(Object); // Despawns o item
                            

                            
                        }
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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AbrirGaveta()
    {
        Debug.Log("Abrindo gaveta");
        transform.localPosition += new Vector3(0, 0, 0.5f); // Move a gaveta para fora
    }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_MostrarOutline()
    {
        GetComponent<Outline>().enabled = true;
    }
}
