using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private Collider2D AttackArea = default;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    public bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AttackArea = gameObject.GetComponentInChildren<PolygonCollider2D>();

        if (AttackArea == null)
        {
            Debug.Log("no attack area");
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Starts timer for delay in between attacks
        if (attacking)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                AttackArea.enabled = false;
            }
        }
         if (isFacingRight)
        {
            AttackArea.transform.localScale = new Vector2(27.23f, 27.23f); // Flip the sprite to face right
        }
        else
        {
             AttackArea.transform.localScale = new Vector2(-27.23f, 27.23f); // Flip the sprite to face left
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //triggers attack when button is hit
        if (context.performed)
        {
            attacking = true;
            AttackArea.enabled = true;
            Debug.Log("Attacking");


        }
    }
}
