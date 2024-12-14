using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerController _playerController;
    PlayerInputs _playerInputs;

    public void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();

        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();

        _playerInputs.Player.Move.performed += OnMovePerformed;
        _playerInputs.Player.Move.canceled += OnMoveCanceled;

        _playerInputs.Player.Look.performed += OnLookPerformed;
        _playerInputs.Player.Look.canceled += OnLookCanceled;

        _playerInputs.Player.Sprint.performed += OnRunPerformed;
        _playerInputs.Player.Sprint.canceled += OnRunCanceled;

        _playerInputs.Player.Jump.performed += OnJumpPerformed;

        _playerInputs.Player.Crouch.performed += OnCrouchPerformed;

        _playerInputs.Player.Interact.performed += OnInteractPerformed;
    }

    public void OnDisable()
    {
        _playerInputs.Player.Move.performed -= OnMovePerformed;
        _playerInputs.Player.Move.canceled -= OnMoveCanceled;

        _playerInputs.Player.Look.performed -= OnLookPerformed;
        _playerInputs.Player.Look.canceled -= OnLookCanceled;

        _playerInputs.Player.Sprint.performed -= OnRunPerformed;
        _playerInputs.Player.Sprint.canceled -= OnRunCanceled;

        _playerInputs.Player.Jump.performed -= OnJumpPerformed;

        _playerInputs.Player.Crouch.performed -= OnCrouchPerformed;

        _playerInputs.Player.Interact.performed -= OnInteractPerformed;

        _playerInputs.Disable();
    }

    // Move
    private void OnMovePerformed(InputAction.CallbackContext context) => _playerController.SetMoveInput(context.ReadValue<Vector2>());
    private void OnMoveCanceled(InputAction.CallbackContext context) => _playerController.SetMoveInput(Vector2.zero);

    // Look
    private void OnLookPerformed(InputAction.CallbackContext context) => _playerController.SetMouseInput(context.ReadValue<Vector2>());
    private void OnLookCanceled(InputAction.CallbackContext context) => _playerController.SetMouseInput(Vector2.zero);

    // Run
    private void OnRunPerformed(InputAction.CallbackContext context) => _playerController.SetRunningValue(true);
    private void OnRunCanceled(InputAction.CallbackContext context) => _playerController.SetRunningValue(false);

    // Jump
    private void OnJumpPerformed(InputAction.CallbackContext context) => _playerController.SetJumpingValue(true);

    // Crouch
    private void OnCrouchPerformed(InputAction.CallbackContext context) => _playerController.Crouch();

    // Interact
    private void OnInteractPerformed(InputAction.CallbackContext context) => _playerController.Interact();
}

