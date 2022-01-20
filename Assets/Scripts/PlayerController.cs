using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings and Direction")]
    [SerializeField]private float movementSpeed = 5.0f;
    [SerializeField]private float InputDirection;

    [Header("Properties")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Animation facing and running")]
    private bool isRunning = false;
    private bool isFacingRight = true;
    
    [Header("Ground Check Stuff")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGroundLayer;
    [SerializeField] private float groundCheckRadious;
    private bool isGrounded;

    [Header("Jumping Settings")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    static public int availableJumps = 1;
    private int availableJumpsRemaining;
    private bool canJump;

    [Header("Wall Jump")]
    static public bool canWallJump = false;
    [SerializeField] private float wallSlidingSpeed = -0.45f;
    [SerializeField] private float verticalWallForce;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private float wallCheckRadious;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private bool wallHold = false;
    [SerializeField] private bool wallJumping;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private LayerMask whatIsWallLayer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        availableJumpsRemaining = availableJumps;
        wallHold = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();

        //wall jump
        WallJump();

        
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckEnvironment();
    }

    private void CheckInput()
    {
        InputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * InputDirection, rb.velocity.y);
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && InputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && InputDirection > 0)
        {
            Flip();
        }

        if(rb.velocity.x <= -0.5f | rb.velocity.x >= 0.5f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateAnimation()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("wallHold", wallHold);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0);
    }

    private void CheckEnvironment()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadious, whatIsGroundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadious);
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadious);
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 3)
        {
            availableJumpsRemaining = availableJumps;
        }
        if(availableJumpsRemaining <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            availableJumpsRemaining--;
        }
    }

    private void WallJump()
    {
        //variable jump height
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (canWallJump == true)
        {
            isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadious, whatIsWallLayer);
            if (isTouchingWall == true && !isGrounded && InputDirection != 0)
            {
                wallHold = true;
                Debug.Log("Holding the Wall");
            }
            else
            {
                wallHold = false;
            }

            if (wallHold)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }

            if (Input.GetButtonDown("Jump") && wallHold == true)
            {
                wallJumping = true;
                Invoke("SetWallJumpingToFalse", wallJumpTime);
            }

            if (wallJumping == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, verticalWallForce);
            }

            if (Input.GetKey(KeyCode.DownArrow) && wallHold | Input.GetKey(KeyCode.S) && wallHold)
            {
                wallSlidingSpeed = 3f;
                CreateDust();
                Debug.Log("Dust is created");
            }
            else
            {
                wallSlidingSpeed = -0.45f;
            }


        }
    }

    private void SetWallJumpingToFalse()
    {
        wallJumping = false;
        availableJumpsRemaining++;
    }

    private void CreateDust()
    {
        dust.Play();
    }

}
