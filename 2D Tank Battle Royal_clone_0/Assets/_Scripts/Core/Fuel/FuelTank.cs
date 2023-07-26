using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FuelTank : NetworkBehaviour
{
    [field: SerializeField] public float maxFuel { get; private set; } = 100;

    public NetworkVariable<float> TotalFuel = new NetworkVariable<float>();

    private void Start()
    {
        if (!IsServer) return;

        TotalFuel.Value = maxFuel;
    }

    private void Update()
    {
        if (IsServer)
        {
            TotalFuel.Value -= 2 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;

        if (!col.TryGetComponent(out FuelCan coin)) return;

        var collectedCoin = coin.FuelUp();

        if (!IsServer) return;

        TotalFuel.Value += collectedCoin;
    }
}
