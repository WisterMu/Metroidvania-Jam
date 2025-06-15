using UnityEngine;

public class WallJump : PlayerMovement
{
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;
    bool groundcheck;


    void Update()
    {
        
    }


    public float wallSlideSpeed = 2;
    bool isWallSliding;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }


}
