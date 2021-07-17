using UnityEngine;
using UnityEngine.Events;

public class Player_Controller : MonoBehaviour
{
    //Amount of force added when the player jumps
    [SerializeField] private float m_JumpForce = 400f;
    // Amount of maxSpeed to crouching movement. 1 = 100%;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    // How much to smooth out the movement
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    //Whether or not a player can steer while jumping
    [SerializeField] private bool m_AirControl = false;
    // A mask determining what is ground to the player
    [SerializeField] private LayerMask m_WhatisGround;
    // A position marking where to check if the player is grounded
    [SerializeField] private Transform m_GroundCheck;
    // A position marking where to check for ceilings
    [SerializeField] private Transform m_CeilingCheck;
    // A collider that will be disabled when crouching
    [SerializeField] private Collider2D m_CrouchDisableCollider;
    // Whether or not the player can Double Jump
    [SerializeField] private bool m_DoubleJump = false;

    // Radius of the overlap circle determine if grounded
    const float k_GroundedRadius = .2f;
    // Whether or not the player is grounded
    private bool m_Grounded;
    // Radius of the overlap circle to determine if the player can standup
    private float k_CeilingRadius = .2f;

    // the rigid body for player
    private Rigidbody2D m_RigidBody2D;

    // For determining which way the player is currently facing
    private bool m_FacingRight = true;
    // A flag to control the double jump
    private bool m_isDouble = false;

    // Vector velocity
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private void Awake()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatisGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // check if crouching, check to see if the player can standup
        if (!crouch)
        {
            // Keep crouching if the player has ceiling obstacles
            if (m_wasCrouching)
            {
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatisGround))
                {
                    crouch = true;
                }
            }
        }

        // Player can only be navigated when grounded or airControl is true
        if (m_Grounded || m_AirControl)
        {
            //if crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the spped when crouching
                move *= m_CrouchSpeed;

                //Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the player by the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_RigidBody2D.velocity.y);
            // Smoothing it out
            m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


            // if the input is moving player is opposite with the facing position
            // move right & facing left
            if (move > 0 && !m_FacingRight)
            {
                FlipCharacter();
            }
            else if (move < 0 && m_FacingRight)
            {
                FlipCharacter();
            }
        }

        // Jump
        if (jump)
        {
            if (m_DoubleJump)
            {
                if (m_Grounded)
                    m_isDouble = true;
                if (!m_Grounded && m_isDouble)
                {
                    // Add vertical force to the player but less than when player is grounded
                    m_RigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
                    m_isDouble = false;
                }
            }
            // Add vertical force to the player
            if(m_Grounded)
            {
                m_Grounded = false;
                m_RigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }
    private void FlipCharacter()
    {
        // Switch the way the player is facing
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
