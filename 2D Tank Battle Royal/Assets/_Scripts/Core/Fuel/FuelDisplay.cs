using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : NetworkBehaviour
{
    [SerializeField] private FuelTank fuelTank;
    [SerializeField] private Image fuelBarImage;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;

        fuelTank.TotalFuel.OnValueChanged += HandleHealthChange;

        HandleHealthChange(0, fuelTank.TotalFuel.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;

        fuelTank.TotalFuel.OnValueChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(float oldHealth, float newHealth)
    {
        fuelBarImage.fillAmount = (float)newHealth / fuelTank.maxFuel;
    }
}
