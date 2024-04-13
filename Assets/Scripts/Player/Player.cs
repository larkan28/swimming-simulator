using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerUI playerUI;
    public PlayerCamera playerCamera;
    public PlayerInteract playerInteract;
    public PlayerInventory playerInventory;
    public PlayerController playerController;

    Health m_health;
    Animator m_animator;

    void Awake ()
    {
        m_health = GetComponent<Health>();
        m_health.Init();

        m_health.OnDeath += OnDeath;
        m_health.OnTakeDamage += OnTakeDamage;

        playerCamera.Init(this);
        playerInteract.Init(this);
        playerInventory.Init(this);
        playerController.Init(this);

        m_animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable ()
    {
        m_health.OnDeath -= OnDeath;
        m_health.OnTakeDamage -= OnTakeDamage;
    }

    void Update ()
    {
        if (!m_health.IsAlive())
            return;

        playerCamera.Think();
        playerInteract.Think();
        playerInventory.Think();
        playerController.Think();
    }

    void FixedUpdate ()
    {
        if (!m_health.IsAlive())
            return;

        playerController.ThinkFixed();
    }

    void OnTakeDamage ()
    {
        playerUI.UpdateHealth(m_health.GetHealthRatio());
    }
    
    void OnDeath ()
    {
        m_animator.SetBool("Dead", true);
        playerUI.UpdateHealth(m_health.GetHealthRatio());
    }
}
