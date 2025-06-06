using System.Collections;
using Fusion;
using UnityEngine;

public class DoorVault : NetworkBehaviour, IInteractable
{
    NetworkBool open = false;

    [Networked][SerializeField] float rotacaoCorreta00 { get; set; }
    [Networked][SerializeField] float rotacaoCorreta01 { get; set; }
    [Networked][SerializeField] float rotacaoCorreta02 { get; set; }
    [Networked][SerializeField] float rotacaoCorreta03 { get; set; }

    [Networked][SerializeField] float rotacaoDoCodigo00 { get; set; }
    [Networked][SerializeField] float rotacaoDoCodigo01 { get; set; }
    [Networked][SerializeField] float rotacaoDoCodigo02 { get; set; }
    [Networked][SerializeField] float rotacaoDoCodigo03 { get; set; }



    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_OnInteractObject(Inven playerInventory)
    {
        Debug.Log("Interagindo com o porta do cofre");

        

        if (rotacaoCorreta00 == rotacaoDoCodigo00 &&
            rotacaoCorreta01 == rotacaoDoCodigo01 &&
            rotacaoCorreta02 == rotacaoDoCodigo02 &&
            rotacaoCorreta03 == rotacaoDoCodigo03)
        {
            Debug.Log("CÓDIGO CORRETO");
            
            if (!open)
            {
                transform.Rotate(Vector3.up * 90);
                open = true;
                StartCoroutine(Fechar());
            }
        }
        else
        {
            Debug.Log("CÓDIGO INCORRETO");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_CodeChanged(int codigoIndex, float novaRotacao)
    {
        if (codigoIndex == 0)
        {
            rotacaoDoCodigo00 = novaRotacao;
            Debug.Log("Código 00 atualizado: " + rotacaoDoCodigo00);
        }
        else if (codigoIndex == 1)
        {
            rotacaoDoCodigo01 = novaRotacao;
            Debug.Log("Código 01 atualizado: " + rotacaoDoCodigo01);
        }
        else if (codigoIndex == 2)
        {
            rotacaoDoCodigo02 = novaRotacao;
            Debug.Log("Código 02 atualizado: " + rotacaoDoCodigo02);
        }
        else if (codigoIndex == 3)
        {
            rotacaoDoCodigo03 = novaRotacao;
            Debug.Log("Código 03 atualizado: " + rotacaoDoCodigo03);
        }
    }

    IEnumerator Fechar()
    {
        yield return new WaitForSeconds(3f);
        open = false;
        transform.Rotate(Vector3.up * -90);
        Debug.Log("Porta fechada");
    }
}
