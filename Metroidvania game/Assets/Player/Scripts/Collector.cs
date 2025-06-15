using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Upgrade upgrade = collision.GetComponent<Upgrade>();
        if (upgrade != null)
        {
            upgrade.Collect();
        }
    }
}
