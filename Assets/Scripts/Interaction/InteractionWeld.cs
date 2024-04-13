using System.Collections;
using UnityEngine;

public class InteractionWeld : MonoBehaviour, Interactable
{
    public MeshRenderer fixedPipe;

    public Material materialBroken;
    public Material materialFixed;

    public float weldingRequired;
    public float weldingSpeed;

    float m_currWelding;

    void Start ()
    {
        fixedPipe.material = materialBroken;
    }

    IEnumerator Welding (Player _player)
    {
        PlayerUI playerUI = _player.playerUI;

        while (m_currWelding < weldingRequired)
        {
            m_currWelding += weldingSpeed * Time.deltaTime;
            playerUI.UpdateWelding(m_currWelding / weldingRequired);

            yield return new WaitForEndOfFrame();
        }

        _player.playerController.SetStop(false);

        playerUI.ShowWelding(false);
        playerUI.UpdateFixed();

        fixedPipe.material = materialFixed;
    }

    public void Interact (Player _player)
    {
        _player.playerUI.ShowWelding(true);
        _player.playerController.SetStop(true);

        StopCoroutine(Welding(_player));
        StartCoroutine(Welding(_player));
    }

    public bool IsAvailable ()
    {
        return !IsFixed();
    }

    public string GetText ()
    {
        return "Soldar";
    }

    public bool IsFixed ()
    {
        return m_currWelding >= weldingRequired;
    }
}
