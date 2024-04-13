using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FishManager : MonoBehaviour
{
    public float scaleOffset;
    public int minFishes;
    public int maxFishes;

    public Fish[] fishes;
    public Shark shark;

    Bounds m_boxBounds;

    void OnDrawGizmosSelected ()
    {
        if (TryGetComponent(out BoxCollider boxCollider))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + boxCollider.center, boxCollider.size * scaleOffset);
        }
    }

    void Start ()
    {
        if (TryGetComponent(out BoxCollider boxCollider))
            m_boxBounds = new Bounds(transform.localPosition + boxCollider.center, boxCollider.size);

        int fishesCount = Random.Range(minFishes, maxFishes);

        for (int i = 0; i < fishesCount; i++)
            SpawnUnit(fishes[Random.Range(0, fishes.Length)]);

        SpawnUnit(shark);
    }

    void Update ()
    {
        
    }

    public void SpawnUnit (Unit _unit)
    {
        Instantiate(_unit, GetRandomPoint(), Quaternion.identity, transform);
    }

    public Vector3 GetRandomPoint ()
    {
        float x = Random.Range(m_boxBounds.min.x, m_boxBounds.max.x);
        float y = Random.Range(m_boxBounds.min.y, m_boxBounds.max.y);
        float z = Random.Range(m_boxBounds.min.z, m_boxBounds.max.z);

        return new Vector3(x, y * 0.5f, z);
    }
}
