using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField] private TankPlayer player;
    [SerializeField] private TMP_Text displayNameText;

    private void Start()
    {
        HandleDisplayName(string.Empty, player.playerName.Value);

        player.playerName.OnValueChanged += HandleDisplayName;
    }

    private void HandleDisplayName(FixedString32Bytes previousName, FixedString32Bytes newName)
    {
        displayNameText.text = newName.ToString();
    }

    private void OnDestroy()
    {
        player.playerName.OnValueChanged -= HandleDisplayName;
    }
}

