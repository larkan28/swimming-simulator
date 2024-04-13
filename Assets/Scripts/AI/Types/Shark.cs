using UnityEngine;
using TMPro;

public class Shark : Unit
{
    public float attackDamage;
    public float attackDistance;

    public float findTargetRadius;
    public LayerMask layerTarget;

    public TextMeshPro textHealth;

    bool m_isAttacking;
    Hitbox m_hitbox;
    Health m_health;
    Health m_target;
    Animator m_animator;

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findTargetRadius);
    }

    protected override void OnInit ()
    {
        m_target = null;
        m_hitbox = GetComponent<Hitbox>();
        m_health = GetComponent<Health>();
        m_health.Init();

        m_health.OnDeath += OnDeath;
        m_health.OnTakeDamage += OnTakeDamage;

        m_animator = GetComponentInChildren<Animator>();
        SetRandomPoint();
    }

    protected override void OnPointReached ()
    {
        m_isFollowing = false;

        if (m_target == null)
            SetRandomPoint();
    }

    void Update ()
    {
        if (m_isAttacking)
            return;

        if (m_target != null)
            FollowTarget();
        else
        {
            Move();
            FindTarget();
        }
    }

    void FollowTarget ()
    {
        Vector3 pos = m_target.GetPosition();
        Vector3 dir = (pos - transform.position).normalized;

        MoveTowards(dir, moveSpeed);
        RotateTowards(dir, rotateSpeed);

        if (Vector3.Distance(pos, transform.position) <= attackDistance)
            Attack();

        if (!m_target.IsAlive())
            m_target = null;
    }

    void Attack ()
    {
        Vector3 dir = (m_target.GetPosition() - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        m_animator.SetTrigger("Attack");
        m_isAttacking = true;
    }

    void FindTarget ()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, findTargetRadius, layerTarget, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < colliders.Length; i++)
        {
            Health health = colliders[i].GetComponent<Health>();

            if (health != null && health.IsAlive() && health != m_health)
            {
                if (health.TryGetComponent(out PlayerController playerController))
                    m_target = playerController.IsSwimming() ? health : null;
                else
                    m_target = health;

                break;
            }
        }
    }

    void OnDeath ()
    {
        m_animator.SetBool("Dead", true);
        textHealth.text = "Salud: " + Mathf.RoundToInt(m_health.GetHealthRatio() * 100f) + "%";
    }

    void OnTakeDamage ()
    {
        m_animator.SetTrigger("Pain");
        textHealth.text = "Salud: " + Mathf.RoundToInt(m_health.GetHealthRatio() * 100f) + "%";
    }

    public void HitboxDetect ()
    {
        m_hitbox.Detect(attackDamage);
    }

    public void SetAttacking (bool _value)
    {
        m_isAttacking = _value;
    }
}
