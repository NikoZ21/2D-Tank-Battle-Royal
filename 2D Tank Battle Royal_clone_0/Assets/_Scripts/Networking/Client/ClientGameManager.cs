using System;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager : IDisposable
{
    private const string MAINMENU = "Menu";
    private JoinAllocation allocation;
    private NetworkClient _networkCLient;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();

        _networkCLient = new NetworkClient(NetworkManager.Singleton);

        AuthState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthState.Authenticated)
        {
            return true;
        }

        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public async Task StartClientAsync(string text)
    {
        try
        {
            allocation = await Relay.Instance.JoinAllocationAsync(text);
        }
        catch (Exception ex)
        {

            Debug.Log(ex);
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        UserData userData = new UserData()
        {
            userName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name"),
            userAuthId = AuthenticationService.Instance.PlayerId,
        };

        string payLoad = JsonUtility.ToJson(userData);
        byte[] payLoadByte = Encoding.UTF8.GetBytes(payLoad);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payLoadByte;

        NetworkManager.Singleton.StartClient();
    }

    public void Dispose()
    {
        _networkCLient?.Dispose();
    }
}

