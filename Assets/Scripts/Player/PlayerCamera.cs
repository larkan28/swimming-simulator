using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 targetOffset;

    public float swimmingMinAngleY;
    public float swimmingMaxAngleY;
    public float minAngleY;
    public float maxAngleY;
    public float rotateSpeed;
    public float lookSensitivity;

    bool m_isAiming;

    float m_angleX;
    float m_angleY;

    Transform m_cameraHolder;
    Transform m_cameraOrigin;

    PlayerController m_playerController;

    public void Init (Player _player)
    {
        m_cameraHolder = transform.GetChild(0);
        m_cameraOrigin = m_cameraHolder.GetChild(0);

        m_playerController = _player.playerController;
    }

    public void Think ()
    {
        m_isAiming = Input.GetKey(KeyCode.Mouse1);

        m_angleX += Input.GetAxis("Mouse X") * lookSensitivity;
        m_angleY += Input.GetAxis("Mouse Y") * lookSensitivity;
        m_angleY = Mathf.Clamp(m_angleY, minAngleY, maxAngleY);

        FollowTarget();

        if (!m_playerController.IsStopped()) 
        {
            if (m_isAiming || m_playerController.IsSubmerging())
                RotateByMouse();
            else
                RotateByKeys();
        }

        MouseLook();
    }

    void MouseLook ()
    {
        m_cameraHolder.localRotation = Quaternion.Euler(-m_angleY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, m_angleX, 0f);
    }

    void RotateByKeys ()
    {
        Vector3 horDir = Input.GetAxisRaw("Horizontal") * transform.right;
        Vector3 verDir = Input.GetAxisRaw("Vertical") * transform.forward;

        Vector3 newDir = (verDir + horDir).normalized;
        newDir.y = 0f;

        if (newDir == Vector3.zero)
            newDir = followTarget.forward;

        Quaternion newRot = Quaternion.LookRotation(newDir);
        RotateTarget(newRot);
    }

    void RotateByMouse ()
    {
        float x = 0f;
        float y = m_angleX;

        if (m_playerController.IsSwimming())
        {
            x = -Mathf.Clamp(m_angleY, swimmingMinAngleY, swimmingMaxAngleY);

            if (!m_playerController.IsSubmerged() && x < 0f)
                x = 0f;
        }

        Quaternion newRot = Quaternion.Euler(x, y, 0f);
        RotateTarget(newRot);
    }

    void RotateTarget (Quaternion _rotation)
    {
        followTarget.rotation = Quaternion.RotateTowards(followTarget.rotation, _rotation, Time.deltaTime * rotateSpeed);
    }

    void FollowTarget ()
    {
        transform.position = followTarget.position + targetOffset;
    }

    public Transform GetHolder ()
    {
        return m_cameraHolder;
    }

    public Transform GetMainCamera ()
    {
        return m_cameraOrigin;
    }
}
