using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction interactAction;
    private InputAction dodgeAction;
    private InputAction quickSlotAction;
    private InputAction inventoryAction;
    private InputAction reloadAction;
    private InputAction secondaryWeaponAction;

    public Vector2 movement;
    public bool isAttackPressed;
    public bool isInteractPressed;
    public bool isDodgePressed;
    public bool isQuickSlot1Pressed;
    public bool isQuickSlot2Pressed;
    public bool isQuickSlot3Pressed;
    public bool isQuickSlot4Pressed;
    public bool isInventoryPressed;
    public bool isReloadPressed;
    public bool isSecondaryWeaponPressed;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        interactAction = InputSystem.actions.FindAction("Interact");
        dodgeAction = InputSystem.actions.FindAction("Dodge");
        quickSlotAction = InputSystem.actions.FindAction("QuickSlot");
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        reloadAction = InputSystem.actions.FindAction("Reload");
        secondaryWeaponAction = InputSystem.actions.FindAction("SecondaryWeapon");
    }
    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        isAttackPressed = attackAction.IsPressed();
        isInteractPressed = interactAction.WasPressedThisFrame();
        isDodgePressed = dodgeAction.WasPressedThisFrame();
        isInventoryPressed = inventoryAction.WasPressedThisFrame();
        isReloadPressed = reloadAction.WasPressedThisFrame();
        isSecondaryWeaponPressed = secondaryWeaponAction.WasPressedThisFrame();
    }
    private void OnQuickSlot(InputAction.CallbackContext ctx)
    {
        var control = ctx.control;
        if (control == Keyboard.current.digit1Key)
        {
            isQuickSlot1Pressed = true;
            Debug.Log("1");
        }

        else if (control == Keyboard.current.digit2Key)
        {
            isQuickSlot2Pressed = true;
            Debug.Log("2");
        }
        else if (control == Keyboard.current.digit3Key)
        {
            isQuickSlot3Pressed = true;
            Debug.Log("3");
        }
        else if (control == Keyboard.current.digit4Key)
        {
            isQuickSlot4Pressed = true;
            Debug.Log("4");
        }
    }
    private void OnEnable()
    {
        quickSlotAction.performed += OnQuickSlot;
        quickSlotAction.Enable();
    }
    private void OnDisable()
    {
        quickSlotAction.performed -= OnQuickSlot;
        quickSlotAction.Disable();
    }
}
