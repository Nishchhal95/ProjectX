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

    public bool IsPrimaryWeaponKeyPressed()
    {
        return masterInputActions.Player.PrimaryWeapon.triggered;
    }

    public bool IsSecondaryWeaponKeyPressed()
    {
        return masterInputActions.Player.SecondaryWeapon.triggered;
    }

    public bool IsMeleeWeaponKeyPressed()
    {
        return masterInputActions.Player.MeleeWeapon.triggered;
    }

    public bool IsAbility1KeyPressed()
    {
        return masterInputActions.Player.Ability1.triggered;
    }

    public bool IsAbility2KeyPressed()
    {
        return masterInputActions.Player.Ability2.triggered;
    }

    public bool IsAbility3KeyPressed()
    {
        return masterInputActions.Player.Ability3.triggered;
    }

    public bool IsAbility4KeyPressed()
    {
        return masterInputActions.Player.Ability4.triggered;
    }
}
