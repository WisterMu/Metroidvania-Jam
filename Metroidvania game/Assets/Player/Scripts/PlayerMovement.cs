using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement and attack
    public PlayerAttack playerAttack;
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 5f;
    float horizontalMovement;

    //Jumping
    public float jumpPower = 10f;   
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    //Falling
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    //Wall check when climbing
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;
    bool groundCheck;

    //Wall slide variables
    public float wallSlideSpeed = 2;
    bool isWallSliding;  

    //Variables to flip sprite
    private SpriteRenderer spriteRenderer; // SpriteRenderer component for flipping the player sprite
    private bool isFacingRight = true; // Flag to check if the enemy is facing right

    private Collider2D CheckWall = default;


    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CheckWall = gameObject.GetComponentInChildren<CircleCollider2D>();
    }

    void Update()
    {
        //Horizontal movement
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

        //Animation triggers
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);

        Gravity();
        ProcessWallSlide();

        // Update the sprite based on the facing direction
        if (isFacingRight)
        {
          
            CheckWall.transform.localScale = new Vector2(0.045f, 0.036f);
        }
        else
        {
           
            CheckWall.transform.localScale = new Vector2(-0.045f, 0.036f);
        }

        playerAttack.isFacingRight = isFacingRight;
    }   

    public void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //fall increasingly faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed)); //max fall speed so they don't fall past camera
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessWallSlide()
    {
        //Not grounded & On a Wall & movement !=0
        if (!groundCheck & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed)); //caps fall rate)
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        //movement left and right
        horizontalMovement = context.ReadValue<Vector2>().x;

        if (horizontalMovement > 0)
        {
            isFacingRight = true; // Player is moving right
        }
        else if (horizontalMovement < 0)
        {
            isFacingRight = false; // Player is moving left
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Only triggers jump when grounded
    if (isGrounded())
    {
        if (context.performed)
        {
            rb.linearVelocity += new Vector2(0, jumpPower);

            animator.SetTrigger("jump");
        }
    }

        if (context.canceled) //makes player fall when spacebar let go
        {
            rb.linearVelocity += new Vector2(0, -(jumpPower * 0.5f));
            animator.SetTrigger("jump");
         }
    }

    public bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            groundCheck = true;
            return true;
        }
        else
        {
            groundCheck = false;
            return false;
        }
        
    }

    //Groundcheck so player can't fly by spamming jump
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        // Draw the player's collider
        Gizmos.DrawWireCube(gameObject.transform.position, gameObject.transform.localScale);

        //Draws wall check
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
   

}
