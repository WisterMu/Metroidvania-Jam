using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int health = 5; // Enemy's health
    [SerializeField]
    private float invulFrameDuration = 1f; // Duration of invulnerability after taking damage in seconds
    private float invulTimer = 0f; // Timer for invulnerability frames
    private bool isInvulnerable = false; // Flag to check if enemy is invulnerable
    private BoxCollider2D damageHitbox; // Hitbox for detecting damage

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageHitbox = GetComponent<BoxCollider2D>();
        if (damageHitbox == null)
        {
            Debug.LogError("Damage hitbox is not assigned or missing on the enemy.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvulnerable)
        {
            invulTimer += Time.deltaTime;
            // Flash the enemy sprite to indicate invulnerability
            GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0.7f), new Color(1f, 0, 0, 0.9f), Mathf.PingPong(Time.time * 4, 1));

            if (invulTimer >= invulFrameDuration)
            {
                Debug.Log("Enemy is no longer invulnerable.");
                isInvulnerable = false;
                invulTimer = 0f;
                // Reset enemy sprite color to normal
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }

        // Collision check for trigger zones
        // Needs to be called in Update to continuously check for collisions (goes to sleep using OnTriggerStay)
        checkForTriggerZone();
    }

    private void takeDamage()
    {
        if (isInvulnerable)
        {
            return;     // Ignore damage if enemy is invulnerable
        }
        health--;
        isInvulnerable = true;
        invulTimer = 0f; // Reset the invulnerability timer
        Debug.Log("Enemy took damage! Current health: " + health);
        if (health <= 0)
        {
            Debug.Log("Enemy is dead!");
            Destroy(gameObject); // Destroy the enemy game object
        }
    }

    private void checkForTriggerZone()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, damageHitbox.size * transform.localScale, 0f, LayerMask.GetMask("Attack"));
        if (hit != null && hit.CompareTag("Attack"))
        {
            // Debug.Log("Enemy is colliding with player attack!");
            Debug.Log("Enemy is colliding with damage hitbox: " + hit.name);
            takeDamage();
        }
    }
}
