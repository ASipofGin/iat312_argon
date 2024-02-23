// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     private float horizontal;
//     [SerializeField]
//     private float speed = 8f;
//     [SerializeField]
//     private float jumpingPower = 16f;
//     private bool isFacingRight = true;

//     private float coyoteTime = 0.15f;
//     private float coyoteTimeCounter;

//     private float jumpBufferTime = 0.15f;
//     private float jumpBufferCounter;

//     [SerializeField] private Rigidbody2D rb;
//     [SerializeField] private Transform groundCheck;
//     [SerializeField] private LayerMask groundLayer;

//     private Transform _originalParent;

//     void Start()
//     {
//         _originalParent = transform.parent;
//     }

//     void Update()
//     {
//         if (IsGrounded())
//         {
//             coyoteTimeCounter = coyoteTime;
//         }else{
//             coyoteTimeCounter -= Time.deltaTime;
//         }

//         if (Input.GetButtonDown("Jump")){
//             jumpBufferCounter = jumpBufferTime;
//         }else{
//             jumpBufferCounter -= Time.deltaTime;
//         }

//         horizontal = Input.GetAxisRaw("Horizontal");

//         if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
//             jumpBufferCounter = 0f;
//         }

//         if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

//             coyoteTimeCounter = 0f;
//         }

//         Flip();
//     }

//     private void FixedUpdate()
//     {
//         rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
//     }

//     private bool IsGrounded()
//     {
//         return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//     }

//     private void Flip()
//     {
//         if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
//         {
//             isFacingRight = !isFacingRight;
//             Vector3 localScale = transform.localScale;
//             localScale.x *= -1f;
//             transform.localScale = localScale;
//         }
//     }

//     public void SetParent (Transform newParent)
//     {
//         _originalParent = transform.parent;
//         transform.parent = newParent;
//     }

//     public void ResetParent()
//     {
//         transform.parent = _originalParent;
//     }

//     public void ResetFace()
//     {
//         isFacingRight = true;
//     }
// }
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    public bool grounded;

    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashRecoveryTime = 0.5f; // Recovery time after dashing
    [SerializeField] private float verticalDashBoost = 5f; // Adjustable vertical boost for the dash
    private bool isDashing = false;
    private bool canControl = true; // Control flag
    private float dashTimeLeft;
    private float dashRecoveryLeft; // Recovery countdown

    private bool wasDashing = false; // Track if the player was dashing in the previous frame

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D playerCollider;

    [SerializeField] private PhysicsMaterial2D idleMaterial;
    [SerializeField] private PhysicsMaterial2D movingMaterial;

    private Transform _originalParent;

    void Start()
    {
        _originalParent = transform.parent;
    }

    void Update()
    {

        grounded = IsGrounded();





        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (wasDashing) // Reset momentum if player was dashing and has now landed
            {
                rb.velocity = new Vector2(0, rb.velocity.y); // Reset horizontal velocity
                wasDashing = false; // Reset the wasDashing flag
            }

            if (Mathf.Abs(horizontal) > 0.01f)
            {
                // The character is moving
                playerCollider.sharedMaterial = movingMaterial;
            }
            else
            {
                // The character is idle
                playerCollider.sharedMaterial = idleMaterial;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            playerCollider.sharedMaterial = movingMaterial;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (canControl) // Only allow movement input if player can control the character
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canControl)
        {
            StartDash();
        }

        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, verticalDashBoost);
                dashTimeLeft -= Time.deltaTime;
            }
            else
            {
                isDashing = false;
                canControl = false; // Disable control after dashing
                dashRecoveryLeft = dashRecoveryTime; // Start recovery countdown
                wasDashing = true; // Set wasDashing flag to true as the dash has just ended
            }
        }

        if (!canControl && !isDashing)
        {
            if (dashRecoveryLeft > 0)
            {
                dashRecoveryLeft -= Time.deltaTime;
                playerCollider.sharedMaterial = idleMaterial;
            }
            else
            {
                canControl = true; // Re-enable control after recovery time
            }
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && canControl) // Ensure jump only happens if control is allowed
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f && canControl)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (canControl) // Only allow flipping if player can control the character
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing && canControl) // Apply horizontal movement if not dashing and control is allowed
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        // Apply both horizontal and vertical boost for the dash
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, verticalDashBoost);
        rb.gravityScale = 0; // Optionally, remove gravity effect during dash
    }

    public void SetParent(Transform newParent)
    {
        _originalParent = transform.parent;
        transform.parent = newParent;
    }

    public void ResetParent()
    {
        transform.parent = _originalParent;
    }

    public void ResetFace()
    {
        isFacingRight = true;
    }

    void OnDisable()
    {
        rb.gravityScale = 1; // Reset gravity scale when the object is disabled
    }

    public void RestControl()
    {
        canControl = true;
    }
}
