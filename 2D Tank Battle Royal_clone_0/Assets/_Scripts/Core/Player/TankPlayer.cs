using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_Camera;
    [SerializeField] private int m_OwnerPriority = 15;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            m_Camera.Priority = m_OwnerPriority;
        }
    }
}
