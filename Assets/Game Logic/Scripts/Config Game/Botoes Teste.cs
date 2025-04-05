using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BotoesTeste : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host", GUILayout.Width(200), GUILayout.Height(100))) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client", GUILayout.Width(200), GUILayout.Height(100))) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server", GUILayout.Width(200), GUILayout.Height(100))) NetworkManager.Singleton.StartServer();
        }

        GUILayout.EndArea();
    }
}
