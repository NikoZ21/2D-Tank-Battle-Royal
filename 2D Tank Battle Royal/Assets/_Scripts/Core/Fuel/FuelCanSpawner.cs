using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FuelCanSpawner : NetworkBehaviour
{
    [SerializeField] private FuelCan[] fuelCans;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        foreach (var fuelCan in fuelCans)
        {
            fuelCan.OnCollected += HandleFuelCollected;
        }
    }

    private void HandleFuelCollected(FuelCan fuelCan)
    {
        fuelCan.transform.position = fuelCan.GetSpawnPoint();
        fuelCan.Reset();
    }
}
