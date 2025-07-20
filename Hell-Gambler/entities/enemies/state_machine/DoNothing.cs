using Godot;
using System;

public partial class DoNothing : Node, IInputBehavior {
  public event EventHandler<AttackEventArgs> OnTryAttack;

  Vector2 IMovementInput.GetMoveInput() {
    return Vector2.Zero;
  }

  void IBehavior.OnActivate() {

  }

  void IBehavior.OnDeactivate() {

  }
}
