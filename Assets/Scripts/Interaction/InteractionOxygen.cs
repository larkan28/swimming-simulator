using System.Collections;
using UnityEngine;

public class InteractionOxygen : MonoBehaviour, Interactable
{
    IEnumerator Recharge (Player _player)
    {
        ItemTank tank = _player.playerInventory.GetItem("Tanque de oxigeno") as ItemTank;

        while (tank != null && !tank.IsOxygenFull())
        {
            tank.RecoveryOxygen();
            yield return new WaitForEndOfFrame();
        }

        _player.playerController.SetStop(false);
    }

    public void Interact (Player _player)
    {
        _player.playerController.SetStop(true);

        StopCoroutine(Recharge(_player));
        StartCoroutine(Recharge(_player));
    }

    public string GetText ()
    {
        return "Recargar oxigeno";
    }

    public bool IsAvailable ()
    {
        return true;
    }
}
