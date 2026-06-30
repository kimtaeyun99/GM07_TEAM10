using UnityEngine;

public class PlayerStateRun : PlayerStateBase
{
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        refRb.linearVelocity = Managers.Input.movement * playerBase.moveSpeed;
    }
}
