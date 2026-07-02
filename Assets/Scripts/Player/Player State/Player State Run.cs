using UnityEngine;

public class PlayerStateRun : PlayerStateBase
{
    [SerializeField] private float walkTimer = 0f;
    [SerializeField] private float walkInterval = 2.7f;
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        bool isWalking = Managers.Input.movement.magnitude > 0.1f;
        Managers.PlayerAudio.PlayerWalkLoop(isWalking);

        refRb.linearVelocity = Managers.Input.movement * playerBase.moveSpeed;
    }
}
