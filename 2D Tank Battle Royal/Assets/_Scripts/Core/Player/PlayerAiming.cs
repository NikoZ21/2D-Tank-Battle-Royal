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

        if (_mainCamera != null)
        {
            Debug.Log("I got the camera");
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Debug.Log(_mainCamera.name);

        var mouseWorldPos = _mainCamera.ScreenToWorldPoint(inputReader.AimPosition);

        turretTransform.up = new Vector2(
            mouseWorldPos.x - turretTransform.position.x,
            mouseWorldPos.y - turretTransform.position.y
            );
    }

}
