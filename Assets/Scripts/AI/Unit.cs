using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float minCornerDistance;

    protected FishManager m_manager;
    protected Vector3 m_wayPoint;
    protected bool m_isFollowing;

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_wayPoint, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, m_wayPoint);
    }

    void Start ()
    {
        if (transform.parent != null)
            m_manager = transform.parent.GetComponent<FishManager>();

        if (m_manager == null)
        {
            this.enabled = false;
            return;
        }

        OnInit();
    }

    public void SetRandomPoint ()
    {
        m_wayPoint = m_manager.GetRandomPoint();
        m_isFollowing = true;
    }

    protected void Move ()
    {
        if (!m_isFollowing)
            return;

        Vector3 dir = (m_wayPoint - transform.position).normalized;

        MoveTowards(dir, moveSpeed);
        RotateTowards(dir, rotateSpeed);

        if (Vector3.Distance(m_wayPoint, transform.position) <= minCornerDistance)
            OnPointReached();
    }

    protected void MoveTowards (Vector3 _dir, float _speed)
    {
        transform.position += _dir * _speed * Time.deltaTime;
    }

    protected void RotateTowards (Vector3 _dir, float _speed)
    {
        if (_dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(_dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _speed * Time.deltaTime);
        }
    }

    protected abstract void OnInit ();
    protected abstract void OnPointReached ();
}
