using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleTon clientPrefab;
    [SerializeField] private HostSingleTon hostPrefab;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            return;
        }

        HostSingleTon hostSingleTon = Instantiate(hostPrefab);
        hostSingleTon.CreateHost();

        ClientSingleTon clientSingleTon = Instantiate(clientPrefab);
        bool authenticated = await clientSingleTon.CreateClientAsync();

        if (authenticated)
        {
            clientSingleTon.GameManager.GoToMenu();
        }
    }
}
