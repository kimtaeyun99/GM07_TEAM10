using UnityEngine;
using UnityEngine.Events;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        None = -1, Idle, Run, Dodge, Death
    }
    [SerializeField] private PlayerBase player;
    [SerializeField] private PlayerState playerState = PlayerState.None;
    [SerializeField] private PlayerStateBase[] playerStates;
    [SerializeField] private UnityEvent<PlayerState> OnStateChanged;

    private CharacterController characterController;
    private PlayerAnimationController playerAnimationController;

    private void Awake()
    {
        if(player == null)
        {
            player = GetComponent<PlayerBase>();
        }
        if(characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        if(playerAnimationController == null)
        {
            playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        }
    }
    public void SetState(PlayerState newState)
    {
        if (playerState == newState) return;
        if (playerState != PlayerState.None)
        {
            playerStates[(int)playerState].enabled = false;
        }
        playerState = newState;
        OnStateChanged?.Invoke(playerState);
    }
    private void OnEnable()
    {
        SetState(PlayerState.Idle);
    }
    private void Update()
    {
        if(Managers.Input.movement == Vector2.zero)
        {
            SetState(PlayerState.Idle);
        }
        else if(Managers.Input.movement != Vector2.zero)
        {
            SetState(PlayerState.Run);
        }
    }
}
