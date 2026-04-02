using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash;

    private PlayerInputActions _action;

    private void Awake()
    {
        Instance = this;

        _action = new PlayerInputActions();
        _action.Enable();

        _action.Combat.Attack.started += PlayerAttack_started;
        _action.Player.Dash.performed += PlayerDash_performed;
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _action.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public void DisableMovement()
    {
        _action.Disable();
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerDash_performed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }
}
