using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            camera.Priority = 0;
        }
    }
}
