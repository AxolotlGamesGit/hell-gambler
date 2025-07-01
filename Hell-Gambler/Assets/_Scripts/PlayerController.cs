using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  [SerializeField] Rigidbody2D rigidBody;

  private InputSystem_Actions inputActions;
  private InputAction moveAction;
  private Vector2 moveInput = new Vector2(0, 0);

  [SerializeField] float topSpeed = 3.0f;
  [SerializeField] float friction = 0.9f;

  void Awake() {
    inputActions = new InputSystem_Actions();
  }

  private void Start() {
    Application.targetFrameRate = 60;
  }

  private void Update() {
    moveInput = moveAction.ReadValue<Vector2>();
    float acceleration = (1/friction - 1) * topSpeed; // Magic formula found at https://www.desmos.com/calculator/qkzwobcwyk

    rigidBody.linearVelocity += moveInput.normalized * acceleration;
    rigidBody.linearVelocity *= friction;
  }

  // Input handling stuff
  void OnEnable() {
    moveAction = inputActions.Player.Move;
    moveAction.Enable();
  }

  void OnDisable() {
    moveAction.Disable();
  }
}
