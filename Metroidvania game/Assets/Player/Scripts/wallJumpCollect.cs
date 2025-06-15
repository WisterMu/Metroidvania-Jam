using UnityEngine;

public class wallJumpCollect : MonoBehaviour, Upgrade
{
    public void Collect()
    {
        Destroy(gameObject);
    }
}
