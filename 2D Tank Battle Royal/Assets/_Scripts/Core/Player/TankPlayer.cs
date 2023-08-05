using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_Camera;
    [SerializeField] private int m_OwnerPriority = 15;

    public NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            // var userData =
            //     HostSingleTon.Instance.GameManager.NetworkServer.GetCorrectUserData(OwnerClientId);
            // playerName.Value = userData.userName;
        }

        if (IsOwner)
        {
            m_Camera.Priority = m_OwnerPriority;
        }
    }
}
