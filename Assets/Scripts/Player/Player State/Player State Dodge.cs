using System.Collections;
using UnityEngine;

public class PlayerStateDodge : PlayerStateBase
{
    private void OnEnable()
    {
        StartCoroutine(DodgeCo());
        StartCoroutine(DodgeCooldownCo());
    }
    private IEnumerator DodgeCo()
    {
        playerBase.isDamageable = false;
        Debug.Log("Dodge On");
        yield return playerBase.dodgeDurationWait;
        playerBase.isDamageable = true;
        Debug.Log("Dodge Off");
    }
    private IEnumerator DodgeCooldownCo()
    {
        playerBase.isDodgeable = false;
        yield return playerBase.dodgeCooldownWait;
        playerBase.isDodgeable = true;
    }
}
