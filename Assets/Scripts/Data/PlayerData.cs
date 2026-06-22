using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player 정보")]
    [SerializeField] private string playerName;
    [SerializeField] private PlayerBase playerPrefab;

    [Header("Player Sprite")]
    [SerializeField] private Sprite playerSprite;

    [Header("Player 능력치")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float dodgeCooldown = 3.0f;
    [SerializeField] private float dodgeDuration = 1.0f;

    public string PlayerName { get { return playerName; } }
    public PlayerBase PlayerPrefab { get { return playerPrefab; } }
    public Sprite PlayerSprite { get { return playerSprite; } }
    public int MaxHp { get { return maxHp; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float DodgeCooldown { get { return dodgeCooldown; } }
    public float DodgeDuration {get { return dodgeDuration; } }
}
