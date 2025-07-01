using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour {

  [SerializeField] Rigidbody2D rigidBody;
  [SerializeField] BoxCollider2D groundTrigger;

  [SerializeField] float topSpeed = 10f;      // The maximum horizontal speed in units/s.
  [SerializeField] float acceleration = 1f;   // The maximum horizontal acceleration in units/s.
  [SerializeField] float friction = 0.6f;     // A multiplier applied to speed every frame before adding acceleration.
  private Vector2 velocity = new Vector2(0, 0);

  private InputSystem_Actions inputActions;
  private InputAction moveAction;
  private Vector2 moveInput = new Vector2(0, 0);
  private InputAction jumpAction;
  private bool jumpInput = new bool();

  [SerializeField] float jumpHeight = 10;         // The velocity of the jump in units/s.

  void Awake() {
    inputActions = new InputSystem_Actions();
  }

  void Update() {
    MoveXAxis();
  }

  // Uses play input to accelerate the player in the input direction.
  void MoveXAxis() {
    moveInput = moveAction.ReadValue<Vector2>();
    velocity = rigidBody.linearVelocity;
    float _acceleration = Math.Clamp(moveInput.x, -1, 1) * acceleration;
    float _xVelocity = (velocity.x * friction) + _acceleration;

    if (Math.Abs(_xVelocity) > topSpeed) {
      _xVelocity = Math.Sign(velocity.x) * topSpeed;
    }

    rigidBody.linearVelocityX = _xVelocity;
  }

  void Jump(InputAction.CallbackContext context) {
    bool _isGrounded = groundTrigger.IsTouchingLayers();

    if (_isGrounded) {
      rigidBody.linearVelocityY = jumpHeight;
    }
  }

  // Input handling stuff
  void OnEnable() {
    moveAction = inputActions.Player.Move;
    moveAction.Enable();

    jumpAction = inputActions.Player.Jump;
    jumpAction.Enable();
    jumpAction.performed += Jump;
  }

  void OnDisable() {
    moveAction.Disable();
    jumpAction.Disable();
  }
}
