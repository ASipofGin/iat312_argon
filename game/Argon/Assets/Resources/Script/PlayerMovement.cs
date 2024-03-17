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

    [SerializeField] private bool learnedDash = false;

    private bool wasDashing = false; // Track if the player was dashing in the previous frame

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D playerCollider;

    [SerializeField] private PhysicsMaterial2D idleMaterial;
    [SerializeField] private PhysicsMaterial2D movingMaterial;

    private Transform _originalParent;

    Animator animator;

    ParticleSystem ps;

    public AudioClip dashClip;
    private AudioSource audioSource;

    private float idleTransitionDelay = 0.05f;
    private float currentIdleTransitionTime = 0f;

    void Start()
    {
        _originalParent = transform.parent;
        animator = GetComponent<Animator>();
        ps = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        disableParticle();
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
                currentIdleTransitionTime = idleTransitionDelay;
                if (animator != null){
                animator.SetBool("isMoving", true);                    
                }

            }
            else if (currentIdleTransitionTime > 0)
            {
                // Countdown before transitioning to idle
                currentIdleTransitionTime -= Time.deltaTime;
            }
            else
            {
                // The character is idle
                playerCollider.sharedMaterial = idleMaterial;
                if (animator != null){
                animator.SetBool("isMoving", false);                    
                }

            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            playerCollider.sharedMaterial = movingMaterial;
            if (Mathf.Abs(horizontal) > 0.01f)
            {
                if (animator != null){
                animator.SetBool("isMoving", true);                
                }

            }
            
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && canControl && learnedDash)
        {
            StartDash();
            animator.SetBool("isDashing", true);
        }

        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, verticalDashBoost);
                dashTimeLeft -= Time.deltaTime;
                enableParticle();
            }
            else
            {
                isDashing = false;
                canControl = false; // Disable control after dashing
                disableParticle();
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
                animator.SetBool("isDashing", false);
                
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
        if (audioSource.isPlaying == false)
            {
                audioSource.time = 0.1f;
                audioSource.PlayOneShot(dashClip);
            }
    }

    public void SetParent(Transform newParent)
    {
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

    public void ResetAnimation()
    {
        animator.SetBool("isDashing", false);
    }

    void OnDisable()
    {
        rb.gravityScale = 1; // Reset gravity scale when the object is disabled
    }

    public void RestControl()
    {
        canControl = true;
    }

    public void learnDash(){
        learnedDash = true;
    }

    public void DeactivateDash()
    {
        learnedDash = false;
    }

    private void disableParticle(){
        ParticleSystem.EmissionModule emissionModule = ps.emission;
        emissionModule.enabled = false;
    }

    private void enableParticle(){
        ParticleSystem.EmissionModule emissionModule = ps.emission;
        emissionModule.enabled = true;
    }

    public bool controllable(){
        return canControl;
    }
}
