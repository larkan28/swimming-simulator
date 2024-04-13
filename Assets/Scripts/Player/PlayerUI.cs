using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUI", menuName = "Simulador 3D/PlayerUI", order = 0)]
public class PlayerUI : ScriptableObject
{
    public event Action OnFixedUpdate;
    public event Action<bool> OnWeldingShow;
    public event Action<float> OnHealthUpdate;
    public event Action<float> OnOxygenUpdate;
    public event Action<float> OnWeldingUpdate;
    public event Action<Interactable> OnInteractUpdate;

    public void UpdateHealth (float _ratio)
    {
        OnHealthUpdate?.Invoke(_ratio);
    }

    public void UpdateOxygen (float _ratio)
    {
        OnOxygenUpdate?.Invoke(_ratio);
    }

    public void UpdateInteract (Interactable _interaction)
    {
        OnInteractUpdate?.Invoke(_interaction);
    }

    public void ShowWelding (bool _value)
    {
        OnWeldingShow?.Invoke(_value);
    }

    public void UpdateWelding (float _progress)
    {
        OnWeldingUpdate?.Invoke(_progress);
    }

    public void UpdateFixed ()
    {
        OnFixedUpdate?.Invoke();
    }
}