using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Vector3 offset;
    public float health;

    float m_maxHealth;
    float m_currHealth;

    public event Action OnTakeDamage;
    public event Action OnDeath;

    public void Init ()
    {
        m_currHealth = m_maxHealth = health;
    }

    public void TakeDamage (float _damage, GameObject _inflictor)
    {
        if (!IsAlive() || gameObject == _inflictor)
            return;

        m_currHealth -= _damage;

        if (m_currHealth <= 0f)
        {
            Death();
            return;
        }

        OnTakeDamage?.Invoke();
    }

    public void Death ()
    {
        m_currHealth = 0f;
        OnDeath?.Invoke();
    }

    public bool IsAlive ()
    {
        return m_currHealth > 0f;
    }

    public float GetHealthRatio ()
    {
        return m_currHealth / m_maxHealth;
    }

    public Vector3 GetPosition ()
    {
        return transform.position + offset;
    }
}
