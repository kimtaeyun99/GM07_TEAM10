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
        Managers.PlayerAudio.PlayerDodge();
        playerBase.isDamageable = false;
        Debug.Log("Dodge On");

        float dodgeTimer = 0f;
        Color originalColor = refSpriteRenderer.color;
        while(dodgeTimer < playerBase.dodgeDuration)
        {
            float alpha = Mathf.PingPong(Time.time * 5f, 0.7f) + 0.3f;
            refSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            dodgeTimer += Time.deltaTime;
            yield return null;
        }

        refSpriteRenderer.color = originalColor;
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
