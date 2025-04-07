using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BotoesTeste : MonoBehaviour
{
    public TMP_InputField inputJoinCode;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host", GUILayout.Width(200), GUILayout.Height(100)))
            {
                FindObjectOfType<RelayManager>().StartHostRelay();
            }

            if (GUILayout.Button("Client", GUILayout.Width(200), GUILayout.Height(100)))
            {
                if (inputJoinCode != null)
                {
                    string codigo = inputJoinCode.text;
                    FindObjectOfType<RelayManager>().StartClientRelay(codigo);
                }
                else
                {
                    Debug.LogWarning("InputJoinCode não está atribuído no Inspector!");
                }
            }

            if (GUILayout.Button("Server", GUILayout.Width(200), GUILayout.Height(100)))
            {
                NetworkManager.Singleton.StartServer(); // Usado só se quiser jogar em LAN
            }
        }

        GUILayout.EndArea();
    }
}
