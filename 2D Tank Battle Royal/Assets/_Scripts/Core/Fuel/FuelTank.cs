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
        TotalFuel.Value = maxFuel;
    }

    private void Update()
    {
        if (IsServer)
        {
            TotalFuel.Value -= 10 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;

        if (!col.TryGetComponent(out FuelCan coin)) return;

        var collectedCoin = coin.FuelUp();

        Debug.Log("value of the fuel up " + collectedCoin);

        if (!IsServer) return;

        TotalFuel.Value += collectedCoin;
    }
}
