using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask pickupLayer;
    public float pickupMaxDistance;

    Player m_player;
    PlayerUI m_playerUI;
    Transform m_mainCamera;
    PlayerController m_playerController;
    Interactable m_currInteraction;

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * pickupMaxDistance);
    }

    public void Init (Player _player)
    {
        m_player = _player;
        m_playerUI = _player.playerUI;
        m_mainCamera = _player.playerCamera.GetMainCamera();
        m_playerController = _player.playerController;
    }

    public void Think ()
    {
        CheckInteraction();
        
        if (Input.GetKeyDown(KeyCode.F) && !m_playerController.IsStopped())
            Interact();
    }

    void Interact ()
    {
        if (m_currInteraction != null)
        {
            m_currInteraction.Interact(m_player);
            m_currInteraction = null;

            m_playerUI.UpdateInteract(null);
        }
    }

    void CheckInteraction ()
    {
        Interactable interaction;

        if (Physics.Raycast(m_mainCamera.position, m_mainCamera.forward, out RaycastHit hit, pickupMaxDistance, pickupLayer))
        {
            interaction = hit.collider.GetComponent<Interactable>();

            if (interaction != null && !interaction.IsAvailable())
                interaction = null;
        }
        else
            interaction = null;

        if (m_currInteraction != interaction)
            m_playerUI.UpdateInteract(interaction);

        m_currInteraction = interaction;
    }
}
