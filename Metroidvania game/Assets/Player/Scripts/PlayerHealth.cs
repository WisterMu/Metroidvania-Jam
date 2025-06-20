using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health = 5; // Player's health
    [SerializeField]
    private int invulFrameDuration = 2; // Duration of invulnerability after taking damage in seconds
    private float invulTimer = 0f; // Timer for invulnerability frames
    private bool isInvulnerable = false; // Flag to check if player is invulnerable
    private BoxCollider2D damageHitbox; // Hitbox for detecting damage


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageHitbox = GetComponent<BoxCollider2D>();
        if (damageHitbox == null)
        {
            Debug.LogError("Damage hitbox is not assigned or missing on the player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvulnerable)
        {
            invulTimer += Time.deltaTime;
            // Flash the player sprite to indicate invulnerability
            GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0.7f), new Color(1f, 0, 0, 0.9f), Mathf.PingPong(Time.time * 4, 1));

            if (invulTimer >= invulFrameDuration)
            {
                Debug.Log("Player is no longer invulnerable.");
                isInvulnerable = false;
                invulTimer = 0f;
                // Reset player sprite color to normal
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
            return;     // Ignore damage if player is invulnerable
        }
        health--;
        isInvulnerable = true;
        invulTimer = 0f; // Reset the invulnerability timer
        Debug.Log("Player took damage! Current health: " + health);
        if (health <= 0)
        {
            Debug.Log("Player is dead!");
            // Handle player death (e.g., respawn, game over)
        }
    }

    private void checkForTriggerZone()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, damageHitbox.size * transform.localScale, 0f, LayerMask.GetMask("Enemy"));
        if (hit != null && hit.CompareTag("Enemy"))
        {
            // Debug.Log("Player is colliding with an enemy: " + hit.name);
            takeDamage();
        }
    }
}
