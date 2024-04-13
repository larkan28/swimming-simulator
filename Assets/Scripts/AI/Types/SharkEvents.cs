using UnityEngine;

public class SharkEvents : MonoBehaviour
{
    public Shark shark;

    void AttackEnd ()
    {
        shark.SetAttacking(false);
    }

    void AttackHit ()
    {
        shark.HitboxDetect();
    }

    void Dead ()
    {
        Destroy(shark.gameObject);
    }
}
