
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.Video;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] VideoPlayer videoPlayer;
    public GameObject playerPrefab;
    public GameObject spawnPoint, rawImage;
    [SerializeField] private NetworkRunner runner;
    [SerializeField] sbyte quantidadeDePlayersParaIniciarCutscene = 2; // Quantidade de players para iniciar a cutscene
    [Tooltip("Valor em porcentagem (0 a 100)")]
    [SerializeField] float cutsceneDuration = 9f; // Dura��o da cutscene em segundos


    [SerializeField] NetworkObject prefabSobrado;

    bool jaChegouEm90 = false, ativouVideo = false;

    public override void FixedUpdateNetwork()
    {

        if (runner.ActivePlayers.Count() == quantidadeDePlayersParaIniciarCutscene && !ativouVideo)
        {
            ativouVideo = true;

            rawImage.SetActive(true); // Ativa o canvas do v�deo
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.loopPointReached += OnVideoEnd;
        }



        if (videoPlayer.isPlaying && !jaChegouEm90)
        {
            
            double progresso = (videoPlayer.time / videoPlayer.length) * 100;
            Debug.Log("Progresso do v�deo: " + progresso);
            if (progresso >= cutsceneDuration)
            {
                jaChegouEm90 = true;
                Debug.Log("Chegou em 90% do v�deo!");

                rawImage.SetActive(false);
                videoPlayer.gameObject.SetActive(false);
            }
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        // S� o peer com StateAuthority vai instanciar

        if (player == Runner.LocalPlayer) // ou Runner.IsServer se tiver usando Server/Client
        {
            Debug.Log("ENTROU NOS ESTADOS UNIDOS DE FORMA LEGAL");
            NetworkObject playerObj = Runner.Spawn(playerPrefab, inputAuthority: player);
            playerObj.transform.position = spawnPoint.transform.position; // Define a posi��o do jogador
            Runner.SetPlayerObject(player, playerObj);

            //playerObj.GetComponentInChildren<Camera>().enabled = false;




        }

        // Verifica se � o primeiro jogador (host)
        if (Runner.IsSharedModeMasterClient && Runner.ActivePlayers.Count() <= 1)
        {
            SpawnarObjetosInterativos();
        }
    }
    private void SpawnarObjetosInterativos()
    {
        Runner.Spawn(prefabSobrado, inputAuthority: Runner.LocalPlayer);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("O v�deo terminou!");
        // Faz o que quiser aqui, tipo mudar de cena, esconder o v�deo, etc.
    }

    


}
