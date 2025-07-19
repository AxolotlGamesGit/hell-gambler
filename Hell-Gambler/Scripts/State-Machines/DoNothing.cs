using Godot;
using System;

public partial class DoNothing : Node, IInputState {
  Vector2 IMovementInput.GetMoveInput() {
    return Vector2.Zero;
  }

  bool IInputState.ShouldAttack() {
    return false;
  }

  void IState.OnActivate() {

  }

  void IState.OnDeactivate() {

  }
}
