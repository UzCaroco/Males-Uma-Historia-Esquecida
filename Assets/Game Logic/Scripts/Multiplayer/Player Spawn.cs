
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.Video;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] VideoPlayer videoPlayer;
    public GameObject playerPrefab;
    public GameObject spawnPoint, canvasVideo;
    [SerializeField] private NetworkRunner runner;


    [SerializeField] NetworkObject prefabSobrado, prefabEspelho, prefabBau, prefabPortaSaida, prefabPortasInteriores;

    bool jaChegouEm90 = false, ativouVideo = false;

    public override void FixedUpdateNetwork()
    {

        if (runner.ActivePlayers.Count() > 1 && runner.ActivePlayers.Count() < 3 && !ativouVideo)
        {
            ativouVideo = true;

            videoPlayer.gameObject.SetActive(true);
            videoPlayer.loopPointReached += OnVideoEnd;
        }



        if (videoPlayer.isPlaying && !jaChegouEm90)
        {
            double progresso = videoPlayer.time / videoPlayer.length;

            if (progresso >= 0.9)
            {
                jaChegouEm90 = true;
                Debug.Log("Chegou em 90% do vídeo!");

                canvasVideo.SetActive(false);

                foreach (var x in runner.ActivePlayers)
                {
                    Debug.Log("Checando player: " + x);
                    var networkObject = runner.GetPlayerObject(x); //Percorre os objetos de rede ativos (Players)
                    Debug.Log("NETWORKOBJECT VAZIO??: " + x);

                    if (networkObject != null) //Verifica se o objeto de rede não é nulo
                    {
                        Debug.Log("EXISTE O NETWORKOBJECT");

                        Camera cam = networkObject.GetComponentInChildren<Camera>();

                        if (cam != null)
                        {
                            cam.enabled = true; //Ativa a câmera do jogador
                        }
                    }
                }
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




        }

        // Verifica se é o primeiro jogador (host)
        if (Runner.IsSharedModeMasterClient && Runner.ActivePlayers.Count() <= 1)
        {
            SpawnarObjetosInterativos();
        }
    }
    private void SpawnarObjetosInterativos()
    {
        Debug.Log("ENTROOOOU");
        Runner.Spawn(prefabSobrado);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("O vídeo terminou!");
        // Faz o que quiser aqui, tipo mudar de cena, esconder o vídeo, etc.
    }

    


}
