using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager : IDisposable
{
    private string lobbyId;
    private const string GAME = "Game";
    private Allocation allocation;
    private string joinCode;
    private NetworkServer _networkServer;
    private const int MaxConnections = 20;

    public async Task StartHostAsync()
    {
        try
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }


        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        try
        {
            var lobbyName = $"{PlayerPrefs.GetString(NameSelector.PlayerNameKey, "No One")}'s Lobby";
            var maxPlayers = 8;
            var lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
           {
               {
                   "JoinCode", new DataObject(visibility:  DataObject.VisibilityOptions.Member,
                   value: joinCode)
               }
           };


            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);

            lobbyId = lobby.Id;

            HostSingleTon.Instance.StartCoroutine(HeartbeatLobby(15));
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
            return;
        }

        _networkServer = new NetworkServer(NetworkManager.Singleton);

        UserData userData = new UserData()
        {
            userName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name"),
            userAuthId = AuthenticationService.Instance.PlayerId,
        };

        string payLoad = JsonUtility.ToJson(userData);
        byte[] payLoadByte = Encoding.UTF8.GetBytes(payLoad);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payLoadByte;

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GAME, LoadSceneMode.Single);
    }

    private IEnumerator HeartbeatLobby(float waitTimeSeconds)
    {
        WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return waitForSecondsRealtime;
        }
    }

    public async void Dispose()
    {
        HostSingleTon.Instance.StopCoroutine(nameof(HeartbeatLobby));

        if (string.IsNullOrEmpty(lobbyId))
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }

            lobbyId = string.Empty;
        }

        _networkServer?.Dispose();
    }
}

