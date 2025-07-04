using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class OpenDoors : NetworkBehaviour
{
    [SerializeField] InteriorDoor interiorDoor;

    [SerializeField] NetworkBool activateTrigger = false;
    [SerializeField] NetworkObject guarda;

    //public override void FixedUpdateNetwork()
    //{
    //    float distance = Vector3.Distance(transform.position, guarda.transform.position);

        

    //    if (distance < 10f && !activateTrigger)
    //    {
    //        Debug.Log("ABRIUUUUUUU");
    //        activateTrigger = true;
    //        interiorDoor.RPC_EnemyOpenDoor();
    //        StartCoroutine(WaitForDoorOpen());
    //    }
    //    else if (distance >= 10f && activateTrigger)
    //    {
    //        Debug.Log("FECHOOOOOU");
    //        activateTrigger = false;
    //        interiorDoor.RPC_EnemyCloseDoor();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !activateTrigger)
        {
            Debug.Log("Enemy entered the trigger");
            activateTrigger = true;
            interiorDoor.RPC_EnemyOpenDoor();

            StartCoroutine(WaitForDoorOpen());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && activateTrigger)
        {
            Debug.Log("Enemy exited the trigger");
            activateTrigger = false;
            interiorDoor.RPC_EnemyCloseDoor();
        }
    }

    IEnumerator WaitForDoorOpen()
    {
        yield return new WaitForSeconds(3f);
        if (activateTrigger)
        {
            activateTrigger = false;
            interiorDoor.RPC_EnemyCloseDoor();
        }
    }
}
