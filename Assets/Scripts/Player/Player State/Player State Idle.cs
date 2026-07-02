using UnityEngine;

public class PlayerStateIdle : PlayerStateBase
{
    private void OnEnable()
    {
        refRb.linearVelocity = Vector2.zero;
    }
}
