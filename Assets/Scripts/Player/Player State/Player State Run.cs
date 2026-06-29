using UnityEngine;

public class PlayerStateRun : PlayerStateBase
{
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        Vector2 movedir = new Vector2(Managers.Input.movement.x, Managers.Input.movement.y);
        refRb.linearVelocity = movedir * playerBase.moveSpeed;
    }
}
