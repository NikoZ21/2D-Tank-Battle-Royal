using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform turretTransform;

    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        var mouseWorldPos = _mainCamera.ScreenToWorldPoint(inputReader.AimPosition);

        turretTransform.up = new Vector2(
            mouseWorldPos.x - turretTransform.position.x,
            mouseWorldPos.y - turretTransform.position.y
            );
    }

}
