using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 5f;
    float horizontalMovement;

    public float jumpPower = 10f;
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;


    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

        Gravity();

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
    }   

    public void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //fall increasingly faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        //movement left and right
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {

    if (isGrounded())
    {
            if (context.performed)
            {
                rb.linearVelocity += new Vector2(0, jumpPower);

                animator.SetTrigger("jump");
            }
    }

        if (context.canceled)
        {
            rb.linearVelocity += new Vector2(0, -(jumpPower * 0.5f));
            animator.SetTrigger("jump");
         }
    }

    private bool isGrounded()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    //Groundcheck so player can't fly by spamming jump
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        // Draw the player's collider
        Gizmos.DrawWireCube(gameObject.transform.position, gameObject.transform.localScale);
    }
   

}
