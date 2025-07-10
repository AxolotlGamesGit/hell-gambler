using Godot;
using System;

public partial class PlayerInput : Node, IMovementInput {
  [Export] Node meleeAttackNode;
  private IAttack meleeAttack;
  private event EventHandler _attack;
  Vector2 IMovementInput.GetInput() {
    return Input.GetVector("move_left", "move_right", "move_up", "move_down").Normalized();
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("melee_attack")) {
      _attack.Invoke(this, new EventArgs());
    }
  }

  private void attack(object sender, EventArgs e) {
    meleeAttack.TryAttack();
  }

  public override void _EnterTree() {
    meleeAttack = (IAttack) meleeAttackNode;
    _attack += new EventHandler(attack);
  }
}
