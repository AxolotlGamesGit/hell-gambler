using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerInput : Node, IMovementInput {
  [Export] Node meleeAttackNode;
  private IAttack meleeAttack;

  private event Action _onAttack;
  private Vector2 _mousePosition;
  Vector2 IMovementInput.GetInput() {
    return Input.GetVector("move_left", "move_right", "move_up", "move_down").Normalized();
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("melee_attack")) {
      _onAttack.Invoke();
    }
    else if (@event is InputEventMouseMotion eventMouseMotion) {
      _mousePosition = eventMouseMotion.GlobalPosition - (GetTree().Root.Size / 2);
    }
  }

  async void Attack() {
    float direction = (float)VectorMath.GetRotationFromVector((_mousePosition - ((Node2D)GetParent()).Position));
    await meleeAttack.TryAttack(direction);
  }

  public override void _EnterTree() {
    meleeAttack = (IAttack)meleeAttackNode;
    _onAttack += Attack;
  }
}