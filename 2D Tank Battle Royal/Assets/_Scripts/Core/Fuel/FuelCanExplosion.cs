using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FuelCanExplosion : NetworkBehaviour
{
    [Header("Refrences")]
    [SerializeField] private SpriteRenderer fuelSprite;
    [SerializeField] private ParticleSystem explosionParticle;

    [Header("Settings")]
    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private int explosionDamage = 20;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;

        if (col.TryGetComponent(out DamageDealer dmgDealer))
        {
            HandleExplosionClientRpc();

            DamageNearbyPlayers();
        }
    }

    private void DamageNearbyPlayers()
    {
        var players = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var p in players)
        {
            var health = p.GetComponentInParent<Health>();

            if (!health) continue;

            health.TakeDamage(explosionDamage);
        }
    }

    [ClientRpc]
    private void HandleExplosionClientRpc()
    {
        fuelSprite.enabled = false;

        if (explosionParticle == null) return;

        explosionParticle.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = Color.blue;
    }
}
