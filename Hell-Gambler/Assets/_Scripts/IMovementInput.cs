using System;
using UnityEngine;

public interface IMovementInput {
  Vector2 GetMoveDirection() {
    return new Vector2(0, 0);
  }

  float GetRotation() { 
    return 0;
  }
}
