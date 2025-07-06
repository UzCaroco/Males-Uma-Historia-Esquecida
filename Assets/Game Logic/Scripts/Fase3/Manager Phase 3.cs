using System.Collections;
using Fusion;
using UnityEngine;

public class ManagerPhase3 : NetworkBehaviour
{
    [SerializeField] NetworkObject prefabGuardaCavalaria;

    [Networked] int quantidadeSpawnado { get; set; } = 0; // Contador de guardas da cavalaria spawnados

    public override void Spawned()
    {
        StartCoroutine(Spawnar());
    }

    IEnumerator Spawnar()
    {
        if (quantidadeSpawnado < 10)
        {
            yield return new WaitForSeconds(3f);
            Runner.Spawn(prefabGuardaCavalaria, new Vector3(Random.Range(-5f, -45f), 8.55f, 8f), Quaternion.identity);
        }
        else if (quantidadeSpawnado < 20 && quantidadeSpawnado > 10)
        {
            yield return new WaitForSeconds(2f);
            Runner.Spawn(prefabGuardaCavalaria, new Vector3(Random.Range(-5f, -45f), 8.55f, 8f), Quaternion.identity);
        }
        else if (quantidadeSpawnado < 50 && quantidadeSpawnado > 20)
        {
            yield return new WaitForSeconds(1f);
            Runner.Spawn(prefabGuardaCavalaria, new Vector3(Random.Range(-5f, -45f), 8.55f, 8f), Quaternion.identity);
        }
        else if (quantidadeSpawnado < 200 && quantidadeSpawnado > 50)
        {
            yield return new WaitForSeconds(0.5f);
            Runner.Spawn(prefabGuardaCavalaria, new Vector3(Random.Range(-5f, -45f), 8.55f, 8f), Quaternion.identity);
        }


        quantidadeSpawnado++;
        StartCoroutine(Spawnar()); // Reinicia o processo de spawn
    }
}
