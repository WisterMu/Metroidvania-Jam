using System;
using UnityEngine;

public enum EnemyType
{
    WalkerBasic,
    WalkerChaser,
    FlyerBasic,
    FlyerChaser
}

public class EnemyMovement : MonoBehaviour
{
    // Controls enemy movement in the game

    public EnemyType enemyType;
    [Header("Basic Settings")]
    public float moveSpeed = 3f; // Speed at which the enemy moves
    public float jumpForce = 2f; // Force applied when the enemy jumps
    public Transform groundCheckPos; // Position to check if the enemy is grounded
    [SerializeField]
    private Vector2 lastPosition; // Last position of the enemy for movement calculations
    [SerializeField]
    private Vector2 deltaPosition; // Change in position for movement calculations
    private SpriteRenderer spriteRenderer; // SpriteRenderer component for flipping the enemy sprite

    private Animator animator; // Animator component for animations

    [Header("Walker Chaser Settings")]
    public float chaseRange = 5f; // Range within which the enemy will chase the target
    private Transform target; // Target to follow (e.g., player)
    private Rigidbody2D rb; // Rigidbody component for physics interactions
    private Vector2 movement; // Movement vector for the enemy
    private bool isFacingRight = true; // Flag to check if the enemy is facing right
    private const float flipCooldown = 0.5f; // Cooldown time for flipping the enemy sprite
    private float flipTimer = 0f; // Timer to track the cooldown for flipping
    private const float stuckCooldown = 0.5f; // Time to wait before flipping if stuck
    private float stuckTimer = 0f; // Timer to track if the enemy is stuck

    [Header("Flyer Settings")]
    public Transform flightPath; // Path for the flying enemy to follow


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (groundCheckPos == null)
        {
            Debug.LogError("Ground Check Position is not assigned in the inspector.");
        }

        if (animator == null)
        {
            // No animator for this enemy
            Debug.LogWarning("Animator component not found for " + gameObject.name + ". Enemy will not have animations.");
        }

        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene. Enemy will not chase any target.");
        }

        if (enemyType == EnemyType.FlyerChaser || enemyType == EnemyType.FlyerBasic)
        {
            rb.gravityScale = 0; // Disable gravity for flying enemies
            if (flightPath == null)
            {
                Debug.LogError("Flight Path is not assigned for flying enemy " + gameObject.name + ". Enemy will not fly.");
            }
            else
            {
                // Save its world position/rotation before unparenting
                Vector3 worldPos = flightPath.position;
                Quaternion worldRot = flightPath.rotation;

                // Unparent it so it no longer moves with this object
                flightPath.parent = null;

                // Reapply world position/rotation so nothing shifts visually
                flightPath.position = worldPos;
                flightPath.rotation = worldRot;
            }
        }
        else
        {
            rb.gravityScale = 1; // Enable gravity for walking enemies
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)  // only set if animator exists
        {
            animator.SetFloat("VelocityX", movement.x);  // currently unused
        }

        // Update the sprite based on the facing direction
        if (isFacingRight)
        {
            spriteRenderer.flipX = false; // Flip the sprite to face right
        }
        else
        {
            spriteRenderer.flipX = true; // Flip the sprite to face left
        }
    }

    void FixedUpdate()
    {
        // Physics updates should be done in FixedUpdate
        if (enemyType == EnemyType.WalkerChaser && target != null)
        {
            ChaseTarget();
        }
        else if (enemyType == EnemyType.WalkerBasic)
        {
            Patrol();
        }
        else if (enemyType == EnemyType.FlyerBasic)
        {
            Patrol();
            Fly();
        }

        deltaPosition = rb.position - lastPosition; // Calculate the change in position
        if (Math.Abs(deltaPosition.x) < 0.001f)
        {
            // Debug.Log("Enemy is stuck, checking for movement...");
            // If the enemy is not moving, increase the stuck timer
            if (stuckTimer < stuckCooldown)
            {
                stuckTimer += Time.deltaTime;
            }
        }
        else
        {
            stuckTimer = 0f; // Reset the stuck timer if the enemy is moving
        }

        lastPosition = rb.position; // Update the move vector with the current velocity
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckPos.localScale, 0, LayerMask.GetMask("Floor")))
        {
            return true;
        }
        return false;
    }

    private void Patrol()
    {
        // Basic walking logic for the enemy
        movement.x = moveSpeed * (isFacingRight ? 1 : -1);
        rb.linearVelocity = new Vector2(movement.x, rb.linearVelocity.y);

        if (isGrounded() && UnityEngine.Random.Range(0f, 1f) < 0.001f) // Randomly jump with a small chance
        {
            Debug.Log("Enemy jumping");
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        // Cooldown on flipping to prevent spazzing
        if (flipTimer > 0f)
        {
            flipTimer -= Time.deltaTime;
        }
        else
        {
            if (enemyType == EnemyType.FlyerChaser || enemyType == EnemyType.FlyerBasic)
            {
                return; // Skip the rest of the patrol logic for flying enemies
            }

            // Walkers flip if it reaches the edge of the platform
            if (Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Floor")).collider == null || stuckTimer >= stuckCooldown)
            {
                isFacingRight = !isFacingRight;
                // Sprite flipping done based on movement, not facing direction
                flipTimer = flipCooldown;
                stuckTimer = 0f; // Reset stuck timer when flipping
            }
        }

    }

    private void ChaseTarget()
    {
        // Chasing logic for the enemy
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < chaseRange)
        {
            // Basic walking logic for the enemy
            movement.x = moveSpeed * (isFacingRight ? 1 : -1);
            rb.linearVelocity = new Vector2(movement.x, rb.linearVelocity.y);

            if (isGrounded() && target.position.y > transform.position.y + 0.5f) // Jump if player is above enemy
            {
                Debug.Log("Enemy jumping");
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

            // Cooldown on flipping to prevent spazzing
            if (flipTimer > 0f)
            {
                flipTimer -= Time.deltaTime;
            }
            else
            {
                // Face the player
                if (target.position.x > transform.position.x && !isFacingRight)
                {
                    isFacingRight = true; // Face right
                }
                else if (target.position.x < transform.position.x && isFacingRight)
                {
                    isFacingRight = false; // Face left
                }
            }
        }
        else
        {
            Patrol(); // If the target is out of range, patrol normally
        }
    }

    private void Fly()
    {
        // Flying logic for the enemy
        // Stays within bounds of flight path transform
        if (flightPath != null)
        {
            // If past left edge of flight path, flip direction
            if (transform.position.x < flightPath.position.x - flightPath.localScale.x / 2)
            {
                isFacingRight = true;
            }

            // If past right edge of flight path, flip direction
            if (transform.position.x > flightPath.position.x + flightPath.localScale.x / 2)
            {
                isFacingRight = false;
            }
            
        }

    }
}
