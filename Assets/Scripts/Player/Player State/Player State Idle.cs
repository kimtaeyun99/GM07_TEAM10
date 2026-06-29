using UnityEngine;

public class PlayerStateIdle : PlayerStateBase
{
    private void Update()
    {
        refRb.linearVelocity = Vector2.zero;
    }
}
