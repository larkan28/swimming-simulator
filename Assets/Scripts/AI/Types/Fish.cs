public class Fish : Unit
{
    void Update ()
    {
        Move();
    }

    protected override void OnInit ()
    {
        SetRandomPoint();
    }

    protected override void OnPointReached ()
    {
        m_isFollowing = false;
        SetRandomPoint();
    }
}
