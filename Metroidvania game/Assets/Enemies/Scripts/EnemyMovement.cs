using UnityEngine;

public enum EnemyType
{
    WalkerBasic,
    WalkerChaser,
    Flyer
}

public class EnemyMovement : MonoBehaviour
{
    // Controls enemy movement in the game

    public EnemyType enemyType;
    [Header("Basic Settings")]
    public float moveSpeed = 3f; // Speed at which the enemy moves
    public float jumpForce = 5f; // Force applied when the enemy jumps
    public float groundCheckDistance = 0.1f; // Distance to check for ground beneath the enemy
    public LayerMask groundLayer; // Layer mask to identify ground objects
    
    [Header("Walker Chaser Settings")]
    public float chaseRange = 5f; // Range within which the enemy will chase the target
    public Transform target; // Target to follow (e.g., player)
    private Rigidbody2D rb; // Rigidbody component for physics interactions
    private Vector2 movement; // Movement vector for the enemy
    private bool isFacingRight = true; // Flag to check if the enemy is facing right
    private Animator animator; // Animator component for animations

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
