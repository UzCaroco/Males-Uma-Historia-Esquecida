
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] VideoPlayer videoPlayer;
    public GameObject playerPrefab;
    public GameObject spawnPoint, rawImage;
    [SerializeField] private NetworkRunner runner;
    [SerializeField] sbyte quantidadeDePlayersParaIniciarCutscene = 2; // Quantidade de players para iniciar a cutscene
    [Tooltip("Valor em porcentagem (0 a 100)")]
    [SerializeField] float cutsceneDuration = 9f; // Duração da cutscene em segundos


    [SerializeField] NetworkObject prefabSobrado, prefabCamara;

    public bool jaChegouEm90 = false;
    bool ativouVideo = false;

    public override void FixedUpdateNetwork()
    {

        if (runner.ActivePlayers.Count() == quantidadeDePlayersParaIniciarCutscene && !ativouVideo)
        {
            ativouVideo = true;

            rawImage.SetActive(true); // Ativa o canvas do vídeo
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.loopPointReached += OnVideoFinished;
        }



        if (videoPlayer.isPlaying && !jaChegouEm90)
        {
            
            double progresso = (videoPlayer.time / videoPlayer.length) * 100;
            Debug.Log("Progresso do vídeo: " + progresso);
            if (progresso >= cutsceneDuration)
            {
                jaChegouEm90 = true;
                Debug.Log("Chegou em 90% do vídeo!");

                rawImage.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        // Só o peer com StateAuthority vai instanciar

        if (player == Runner.LocalPlayer) // ou Runner.IsServer se tiver usando Server/Client
        {
            Debug.Log("ENTROU NOS ESTADOS UNIDOS DE FORMA LEGAL");
            NetworkObject playerObj = Runner.Spawn(playerPrefab, inputAuthority: player);
            playerObj.transform.position = spawnPoint.transform.position; // Define a posição do jogador
            Runner.SetPlayerObject(player, playerObj);

            //playerObj.GetComponentInChildren<Camera>().enabled = false;

            var localId = playerObj.GetComponent<PlayerLocalIdentifier>();
            if (localId != null)
                localId.isLocalPlayer = true;


        }

        // Verifica se é o primeiro jogador (host)
        if (Runner.IsSharedModeMasterClient && Runner.ActivePlayers.Count() <= 1)
        {
            SpawnarObjetosInterativos();
        }
    }
    private void SpawnarObjetosInterativos()
    {
        Runner.Spawn(prefabSobrado, inputAuthority: Runner.LocalPlayer);
    }

    public void SpawnaCaralho()
    {
        Debug.Log("Spawna o caralho");
        Runner.Spawn(prefabCamara, inputAuthority: Runner.LocalPlayer);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Cutscene finished, disabling video player and raw image.");
        rawImage.gameObject.SetActive(false);
        vp.gameObject.SetActive(false);
    }

}
