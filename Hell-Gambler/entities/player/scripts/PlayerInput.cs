using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerInput : Node, IMovementInput, IAttackInput {
  public event EventHandler<AttackEventArgs> OnTryAttack;

  [Export] Node meleeAttackNode;
  private IAttack meleeAttack;

  Vector2 _mousePosition;

  Vector2 IMovementInput.GetMoveInput() {
    return Input.GetVector("move_left", "move_right", "move_up", "move_down").Normalized();
  }

  async void Attack() {
    float direction = (float)VectorMath.GetRotationFromVector((_mousePosition - ((Node2D)GetParent()).Position));
    OnTryAttack?.Invoke(this, new AttackEventArgs(direction));
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("melee_attack")) {
      Attack();
    }
    else if (@event is InputEventMouseMotion eventMouseMotion) {
      _mousePosition = eventMouseMotion.GlobalPosition - (GetTree().Root.Size / 2);
    }
  }

  public override void _EnterTree() {
    meleeAttack = (IAttack)meleeAttackNode;
  }
}