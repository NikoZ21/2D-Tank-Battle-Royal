using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead;

    public Action<Health> OnPlayerDied;


    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healthValue)
    {
        ModifyHealth(healthValue);
    }

    private void ModifyHealth(int value)
    {   
        if (isDead) return;

        var modifiedHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(modifiedHealth, 0, MaxHealth);

        if (CurrentHealth.Value <= 0)
        {
            isDead = true;
            OnPlayerDied?.Invoke(this);
        }
    }
}
