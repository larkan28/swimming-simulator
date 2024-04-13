using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public PlayerUI playerUI;

    public Image imageHealthBar;
    public Image imageOxygenBar;
    public Image imageWeldingBar;
    public TextMeshProUGUI textFixed;
    public TextMeshProUGUI textHealth;
    public TextMeshProUGUI textOxygen;
    public TextMeshProUGUI textWelding;
    public TextMeshProUGUI textInteract;
    public GameObject interactObject;
    public GameObject weldingObject;

    void OnEnable ()
    {
        playerUI.OnFixedUpdate += OnFixedUpdate;
        playerUI.OnWeldingShow += OnWeldingShow;
        playerUI.OnHealthUpdate += OnHealthUpdate;
        playerUI.OnOxygenUpdate += OnOxygenUpdate;
        playerUI.OnWeldingUpdate += OnWeldingUpdate;
        playerUI.OnInteractUpdate += OnInteractUpdate;
    }

    void OnDisable ()
    {
        playerUI.OnFixedUpdate -= OnFixedUpdate;
        playerUI.OnWeldingShow -= OnWeldingShow;
        playerUI.OnHealthUpdate -= OnHealthUpdate;
        playerUI.OnOxygenUpdate -= OnOxygenUpdate;
        playerUI.OnWeldingUpdate -= OnWeldingUpdate;
        playerUI.OnInteractUpdate -= OnInteractUpdate;
    }

    void OnHealthUpdate (float _ratio)
    {
        imageHealthBar.fillAmount = _ratio;
        textHealth.text = "Salud: " + Mathf.RoundToInt(_ratio * 100f).ToString() + "%";
    }

    void OnOxygenUpdate (float _ratio)
    {
        imageOxygenBar.fillAmount = _ratio;
        textOxygen.text = "Oxigeno: " + Mathf.RoundToInt(_ratio * 100f).ToString() + "%";
    }

    void OnInteractUpdate (Interactable _interaction)
    {
        interactObject.SetActive(_interaction != null);

        if (interactObject.activeSelf)
            textInteract.text = _interaction.GetText();
    }

    void OnWeldingUpdate (float _progress)
    {
        imageWeldingBar.fillAmount = _progress;
        textWelding.text = "Soldando: " + Mathf.RoundToInt(_progress * 100f).ToString() + "%";
    }

    void OnWeldingShow (bool _value)
    {
        weldingObject.SetActive(_value);
    }

    void OnFixedUpdate ()
    {
        int fixedCount = 0;
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");

        for (int i = 0; i < pipes.Length; i++)
        {
            if (pipes[i].TryGetComponent(out InteractionWeld weld))
                fixedCount += weld.IsFixed() ? 1 : 0;
        }

        textFixed.text = "Caños parchados: " + fixedCount + "/3";
    }
}
