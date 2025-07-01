using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IMovementInput {
  private InputSystem_Actions inputActions;
  private InputAction moveAction;
  private Vector2 moveInput = new Vector2(0, 0);

  Vector2 IMovementInput.GetMoveDirection() {
    moveInput = moveAction.ReadValue<Vector2>();
    return moveInput.normalized;
  }

  void Awake() {
    inputActions = new InputSystem_Actions();
  }

  void OnEnable() {
    moveAction = inputActions.Player.Move;
    moveAction.Enable();
  }

  void OnDisable() {
    moveAction.Disable();
  }
}
