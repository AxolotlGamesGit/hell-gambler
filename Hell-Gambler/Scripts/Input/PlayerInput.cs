using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerInput : Node, IMovementInput {
  [Export] Node meleeAttackNode;
  private IAttack meleeAttack;

  private Vector2 _mousePosition;
  Vector2 IMovementInput.GetInput() {
    return Input.GetVector("move_left", "move_right", "move_up", "move_down").Normalized();
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("melee_attack")) {
      float direction = (float)VectorMath.GetRotationFromVector((_mousePosition - ((Node2D)GetParent()).Position) * new Vector2(1, -1));
      GD.Print((_mousePosition - ((Node2D)GetParent()).Position) * new Vector2(1, -1));
      GD.Print(direction);
      meleeAttack.TryAttack(direction);
    }
    else if (@event is InputEventMouseMotion eventMouseMotion) {
      _mousePosition = eventMouseMotion.GlobalPosition - (GetTree().Root.Size / 2);
    }
  }

  public override void _EnterTree() {
    meleeAttack = (IAttack)meleeAttackNode;
  }
}