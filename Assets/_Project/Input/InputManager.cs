using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance
    {
        get { return instance; }
    }
    private MasterInputActions masterInputActions;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
        masterInputActions = new MasterInputActions();
    }

    private void OnEnable()
    {
        masterInputActions.Enable();
    }

    private void OnDisable()
    {
        masterInputActions.Disable();
    }

    public Vector2 GetPlayerInput()
    {
        return masterInputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return masterInputActions.Player.Look.ReadValue<Vector2>();
    }

    public bool IsJumpPressed()
    {
        return masterInputActions.Player.Jump.triggered;
    }

    public bool IsShiftHeld()
    {
        return masterInputActions.Player.Walk.IsPressed();
    }

    public bool IsShootPressed()
    {
        return masterInputActions.Player.Shoot.triggered;
    }

    public bool IsReloadPressed()
    {
        return masterInputActions.Player.Reload.triggered;
    }
}
