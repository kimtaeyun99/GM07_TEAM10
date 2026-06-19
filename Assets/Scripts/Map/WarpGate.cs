using UnityEngine;
using UnityEngine.Events;

public class WarpGate : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private Collider2D warpTrigger;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private UnityEvent onWarp;

    private void Reset()
    {
        warpTrigger = GetComponentInChildren<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        ApplyState();
    }

    public void Activate()
    {
        isActive = true;
        ApplyState();
    }

    public void Deactivate()
    {
        isActive = false;
        ApplyState();
    }

    private void ApplyState()
    {
        if (warpTrigger != null)
        {
            warpTrigger.enabled = isActive;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isActive;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        onWarp?.Invoke();
    }
}
