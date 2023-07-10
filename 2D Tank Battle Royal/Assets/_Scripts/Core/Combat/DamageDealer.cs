using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;

    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.attachedRigidbody == null) return;

        if (!col.attachedRigidbody.TryGetComponent(out NetworkObject netbj)) return;

        if (ownerClientId == netbj.OwnerClientId) return;


        if (col.attachedRigidbody.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
