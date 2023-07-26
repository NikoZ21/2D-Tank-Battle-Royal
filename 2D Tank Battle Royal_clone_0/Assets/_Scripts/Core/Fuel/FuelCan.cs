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

    public float FuelUp()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

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
