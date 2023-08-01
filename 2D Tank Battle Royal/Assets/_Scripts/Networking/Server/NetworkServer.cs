using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;
    private Dictionary<ulong, string> clientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> authIdToUserData = new Dictionary<string, UserData>();

    public NetworkServer(NetworkManager networkManager)
    {
        _networkManager = networkManager;

        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        _networkManager.OnServerStarted += OnNetworkReady;
    }

    private void OnNetworkReady()
    {
        _networkManager.OnClientConnectedCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (clientIdToAuth.TryGetValue(clientId, out string authId))
        {
            clientIdToAuth.Remove(clientId);
            authIdToUserData.Remove(authId);
        }
    }

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {

        string payLoad = System.Text.Encoding.UTF8.GetString(request.Payload);

        UserData userData = JsonUtility.FromJson<UserData>(payLoad);

        clientIdToAuth[request.ClientNetworkId] = userData.userAuthId;
        authIdToUserData[userData.userAuthId] = userData;

        response.Approved = true;
    }

    public void Dispose()
    {
        if (_networkManager == null) return;

        _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        _networkManager.OnServerStarted -= OnNetworkReady;

        if (_networkManager.IsListening) _networkManager.Shutdown();
    }
}
