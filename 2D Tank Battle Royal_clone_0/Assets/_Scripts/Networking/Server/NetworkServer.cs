using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkServer
{
    private NetworkManager _networkManager;

    public NetworkServer(NetworkManager networkManager)
    {
        _networkManager = networkManager;

        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {

        string payLoad = System.Text.Encoding.UTF8.GetString(request.Payload);

        UserData userData = JsonUtility.FromJson<UserData>(payLoad);

        Debug.Log(userData.userName);

        response.Approved = true;
    }
}
