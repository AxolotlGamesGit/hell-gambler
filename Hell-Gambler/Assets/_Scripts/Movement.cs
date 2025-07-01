using UnityEngine;

public class Movement : MonoBehaviour {
  [SerializeField] Rigidbody2D rigidBody;

  [SerializeField] IMovementInput input;

  [SerializeField] float topSpeed = 3.0f;
  [SerializeField] float friction = 0.9f;

  private void Awake() {
    input = GetComponent<IMovementInput>();
    if (input == null) {
      Debug.LogError("Didn't find an input script.");
    }

    Application.targetFrameRate = 60;
  }

  private void Update() {
    Vector2 moveInput = input.GetMoveDirection().normalized;
    float acceleration = (1 / friction - 1) * topSpeed; // Magic formula found at https://www.desmos.com/calculator/qkzwobcwyk
    rigidBody.linearVelocity += moveInput * acceleration;
    rigidBody.linearVelocity *= friction;

    transform.rotation = Quaternion.Euler(0, 0, input.GetRotation());
  }
}
