using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class CoinCollector : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;

        if (!col.TryGetComponent(out Coin coin)) return;

        var collectedCoin = coin.Collect();

        if (!IsServer) return;

        TotalCoins.Value += collectedCoin;

    }
}
