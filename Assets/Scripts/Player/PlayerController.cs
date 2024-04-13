using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Swimming")]
    public LayerMask layerWater;

    public Vector3 swimBoxSize;
    public Vector3 swimBoxOffset;

    public Vector3 headBoxSize;
    public Vector3 headBoxOffset;

    [Header("Character Controller")]
    public Vector3 swimControllerCenter;
    public float swimControllerHeight;

    [Header("Floating")]
    public Vector3 floatingOffset;
    public float floatingDistance;

    [Header("Movement")]
    public float swimSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float gravity;

    bool m_isRunning;
    bool m_isSwimming;
    bool m_isSubmerged;
    bool m_lastSwimming;
    bool m_lastSubmerged;
    bool m_isFloatingUp;
    bool m_isStopped;

    float m_forwardSpeed;
    float m_verticalSpeed;
    Vector2 m_inputAxis;
    Animator m_animator;
    CharacterController m_controller;

    float defaultControllerHeight;
    Vector3 defaultControllerCenter;

    const float k_groundAcceleration = 2f;

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + swimBoxOffset, swimBoxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + headBoxOffset, headBoxSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + floatingOffset, Vector3.down * floatingDistance);
    }

    public void Init (Player _player)
    {
        m_animator = GetComponent<Animator>();
        m_controller = GetComponent<CharacterController>();

        defaultControllerHeight = m_controller.height;
        defaultControllerCenter = m_controller.center;
    }

    public void Think ()
    {
        m_isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        m_inputAxis = new Vector2(hor, ver);
        m_animator.SetFloat("Speed", m_forwardSpeed);

        m_animator.SetBool("Swimming", m_isSwimming);
        m_animator.SetBool("Submerged", m_isSubmerged);

        CheckWaterTrigger();
        CheckControllerSize();
    }

    public void ThinkFixed ()
    {
        if (m_isStopped)
            return;

        SetForwardSpeed();
        SetVerticalSpeed();
    }

    void CheckWaterTrigger ()
    {
        Vector3 bodyCenter = transform.position + swimBoxOffset;
        Vector3 bodySize = swimBoxSize * 0.5f;

        m_isSwimming = Physics.CheckBox(bodyCenter, bodySize, Quaternion.identity, layerWater, QueryTriggerInteraction.Collide);

        if (!m_isFloatingUp)
        {
            Vector3 headCenter = transform.position + headBoxOffset;
            Vector3 headSize = headBoxSize * 0.5f;

            m_isSubmerged = Physics.CheckBox(headCenter, headSize, Quaternion.identity, layerWater, QueryTriggerInteraction.Collide);
        }
        else
        {
            Vector3 rayPos = transform.position + floatingOffset;
            Vector3 rayDir = Vector3.down;

            if (!Physics.Raycast(rayPos, rayDir, floatingDistance, layerWater, QueryTriggerInteraction.Collide))
                m_isFloatingUp = false;
        }
    
        if (m_lastSubmerged && !m_isSubmerged)
            m_isFloatingUp = true;

        m_lastSubmerged = m_isSubmerged;
    }

    void CheckControllerSize ()
    {
        if (m_lastSwimming && !m_isSwimming)
        {
            m_controller.center = defaultControllerCenter;
            m_controller.height = defaultControllerHeight;
        }
        else if (!m_lastSwimming && m_isSwimming)
        {
            m_controller.center = swimControllerCenter;
            m_controller.height = swimControllerHeight;
        }

        m_lastSwimming = m_isSwimming;
    }

    void SetVerticalSpeed ()
    {
        if (m_controller.isGrounded)
            m_verticalSpeed = -10f;
        else
            m_verticalSpeed -= gravity * Time.deltaTime;
    }

    void SetForwardSpeed ()
    {
        float axisMagnitude = Mathf.Clamp01(m_inputAxis.magnitude);
        float desiredSpeed;

        if (m_isSwimming)
            desiredSpeed = swimSpeed;
        else if (m_isRunning)
            desiredSpeed = runSpeed;
        else
            desiredSpeed = walkSpeed;

        m_forwardSpeed = Mathf.MoveTowards(m_forwardSpeed, desiredSpeed * axisMagnitude, Time.deltaTime * k_groundAcceleration);
    }

    void OnAnimatorMove ()
    {
        if (m_isStopped)
            return;

        Vector3 moveDir;

        if (m_isSwimming)
        {
            moveDir = transform.forward * m_forwardSpeed * Time.deltaTime;

            if (m_isFloatingUp)
                moveDir += Vector3.up * swimSpeed * Time.deltaTime;
        }
        else
        {
            if (m_controller.isGrounded)
            {
                RaycastHit rayHit;

                if (Physics.Raycast(transform.position, Vector3.down, out rayHit, 1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                    moveDir = Vector3.ProjectOnPlane(m_animator.deltaPosition, rayHit.normal);
                else
                    moveDir = m_animator.deltaPosition;
            }
            else
                moveDir = transform.forward * m_forwardSpeed * Time.deltaTime;

            if (!m_isSwimming)
                moveDir += Vector3.up * m_verticalSpeed * Time.deltaTime;
        }        

        m_controller.Move(moveDir);
    }

    public bool IsSwimming ()
    {
        return m_isSwimming;
    }

    public bool IsSubmerged ()
    {
        return m_isSubmerged;
    }

    public bool IsSubmerging ()
    {
        return m_isSwimming && m_isRunning && !m_isFloatingUp;
    }

    public void SetStop (bool _value)
    {
        m_isStopped = _value;

        if (_value)
        {
            m_forwardSpeed = 0f;
            m_verticalSpeed = 0f;
        }
    }

    public bool IsStopped ()
    {
        return m_isStopped;
    }
}
