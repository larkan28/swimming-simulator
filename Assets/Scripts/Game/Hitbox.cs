using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Vector3 size;
    public Vector3 offset;

    public LayerMask layerTarget;

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }

    public void Detect (float _damage)
    {
        Vector3 center = transform.position + (transform.rotation * offset);
        Vector3 halfSize = size * 0.5f;

        Collider[] colliders = Physics.OverlapBox(center, halfSize, transform.rotation, layerTarget);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Health health))
                health.TakeDamage(_damage, gameObject);
        }
    }
}
