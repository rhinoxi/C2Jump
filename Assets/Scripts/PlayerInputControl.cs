using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl : MonoBehaviour
{
    public Controller controller;
    PlayerInput input;

    private void Awake() {
        input = new PlayerInput();
        input.PlayerControl.Move.performed += HandleMove;
        input.PlayerControl.Jump.started += HandleJump;
        input.PlayerControl.Jump.canceled += HandleJumpRelease;
        input.PlayerControl.Grab.started += HandleGrab;
        input.PlayerControl.Grab.canceled += HandleGrabRelease;
        input.PlayerControl.Dash.started += HandleDash;
    }

    private void OnEnable() {
        input.PlayerControl.Enable();
    }

    public void HandleMove(InputAction.CallbackContext ctx) {
        controller.Movement = ctx.ReadValue<Vector2>();
    }

    public void HandleJump(InputAction.CallbackContext ctx) {
        controller.Jump = true;
    }

    public void HandleJumpRelease(InputAction.CallbackContext ctx) {
        controller.Jump = false;
    }

    public void HandleGrab(InputAction.CallbackContext ctx) {
        controller.Grab = true;
    }

    public void HandleGrabRelease(InputAction.CallbackContext ctx) {
        controller.Grab = false;
    }

    public void HandleDash(InputAction.CallbackContext ctx) {
        controller.Dash = true;
    }
}
