using UnityEngine;

public class ItemTank : Item
{
    [Header("Tank")]
    public PlayerUI playerUI;
    public float oxygenMax;
    public float oxygenRecovery;
    public float oxygenDecrease;
    public float noOxygenDamage;
    public float noOxygenCooldown;

    float m_lastNoOxygenTime;
    float m_currOxygen;
    float m_maxOxygen;

    Health m_playerHealth;
    PlayerController m_playerController;

    public override void Init (Player _player)
    {
        base.Init(_player);

        m_playerHealth = _player.GetComponent<Health>();
        m_playerController = _player.playerController;

        m_currOxygen = 0f;
        m_maxOxygen = oxygenMax;

        playerUI.UpdateOxygen(GetOxygenRatio());
    }

    public override void Use ()
    {
        if (m_playerController.IsSubmerged())
        {
            ConsumeOxygen();

            if (m_currOxygen <= 0f)
                DamagePlayer();
        }
    }

    void DamagePlayer ()
    {
        float time = Time.time;

        if (time > m_lastNoOxygenTime)
        {
            m_playerHealth.TakeDamage(noOxygenDamage, null);
            m_lastNoOxygenTime = time + noOxygenCooldown;
        }
    }

    void ConsumeOxygen ()
    {
        if (m_currOxygen > 0f)
        {
            m_currOxygen -= oxygenDecrease * Time.deltaTime;

            if (m_currOxygen < 0f)
                m_currOxygen = 0f;

            playerUI.UpdateOxygen(GetOxygenRatio());
        }
    }

    public void RecoveryOxygen ()
    {
        if (m_currOxygen < m_maxOxygen)
        {
            m_currOxygen += oxygenRecovery * Time.deltaTime;

            if (m_currOxygen > m_maxOxygen)
                m_currOxygen = m_maxOxygen;

            playerUI.UpdateOxygen(GetOxygenRatio());
        }
    }

    public bool IsOxygenFull ()
    {
        return m_currOxygen >= m_maxOxygen;
    }

    float GetOxygenRatio ()
    {
        return m_currOxygen / m_maxOxygen;
    }
}