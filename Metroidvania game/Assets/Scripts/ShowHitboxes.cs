using UnityEngine;

public class ShowHitboxes : MonoBehaviour
{
    // This script is used to visualize the hitboxes of the player in the Unity Editor.
    public bool showHitboxes = true; // Toggle to show or hide hitboxes in the editor

    public Transform[] transforms;  // Optional array of transforms to visualize (like ground check positions)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnDrawGizmos()
    {
        if (!showHitboxes)
        {
            return; // Exit if hitboxes are not set to be shown
        }
        // Get all colliders from this GameObject
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();

        // Debug.Log("Found " + colliders.Length + " colliders on this GameObject.");

        if (colliders.Length == 0)
        {
            Debug.LogWarning("No colliders found on this GameObject. Please add a collider to visualize hitboxes.");
        }

        Color[] colors; // Array to hold colors for each collider
        if (transforms.Length == 0)
        {
            colors = new Color[colliders.Length]; // Array to hold colors for each collider
        }
        else
        {
            colors = new Color[colliders.Length + transforms.Length]; // Array to hold colors for each collider
        }


            // Assign a unique color to each collider for visualization
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(Random.value, Random.value, Random.value, 1);
            }
        // Gizmos.DrawWireCube(transform.position, transform.localScale);
        // Draw the player's collider
        int j = 0;
        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                Gizmos.color = colors[j];
                // Draw a wireframe cube for each collider
                Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
                j++;
            }
        }
        
        if (transforms.Length == 0)
        {
            return; // Exit if no transforms are provided
        }
        foreach (Transform transform in transforms)
        {
            if (transform != null)
            {
                Gizmos.color = colors[j];
                // Draw a wireframe cube for each transform
                Gizmos.DrawWireCube(transform.position, transform.localScale);
                j++;
            }
        }
    }
}
