using System;
using Unity.Netcode;
using UnityEngine;

public class FuelCan : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action<FuelCan> OnCollected;

    private Vector3 spawnPoint;

    private float fuelValue = 10;

    [SerializeField] private bool alreadyCollected;

    public Vector3 GetSpawnPoint() => spawnPoint;

    public void SetValue(float value)
    {
        fuelValue = value;
    }

    public void Update()
    {
        if (!IsServer) return;

        Debug.Log(" I am on server and u can pick me up");
    }

    public float FuelUp()
    {
        if (!IsServer)
        {
            Debug.Log("not showing cause this is a server");
            Show(false);
            return 0;
        }

        Debug.Log("this is serber fueling up");

        if (alreadyCollected) return 0;

        alreadyCollected = true;

        return fuelValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
        Show(true);
    }

    private void Show(bool state)
    {
        spriteRenderer.enabled = state;
    }
}
