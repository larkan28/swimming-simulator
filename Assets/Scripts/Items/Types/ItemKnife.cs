using UnityEngine;

public class ItemKnife : Item
{
    public float damage;

    Animator m_animator;
    bool m_isAttacking;

    public override void Init (Player _player)
    {
        base.Init(_player);
        m_animator = _player.GetComponent<Animator>();
    }

    public override void Use ()
    {
        if (!m_isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
    }

    void Attack ()
    {
        m_isAttacking = true;
        m_animator.SetTrigger("Attack");
    }

    public void SetAttacking (bool _value)
    {
        m_isAttacking = _value;
    }
}
