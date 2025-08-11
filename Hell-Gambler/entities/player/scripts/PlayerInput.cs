using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerInput : Node, IMovementInput, IAttackInput {
  public event EventHandler<AttackEventArgs> OnTryAttack;

  [ExportGroup("Parameters")]
  [Export] float rollSpeed;
  [Export] float rollDuration;

  private Vector2 _mousePosition;
  private event Action _onRoll;
  private float _speedMultiplier = 1f;

  Vector2 IMovementInput.GetMoveInput() {
    return Input.GetVector("move_left", "move_right", "move_up", "move_down").Normalized() * _speedMultiplier;
  }

  float IAttackInput.GetLookDirection() {
    return (float)VectorMath.GetRotationFromVector(_mousePosition - ((Node2D)GetParent()).Position);
  }

  void Attack() {
    float direction = ((IAttackInput)this).GetLookDirection();
    OnTryAttack?.Invoke(this, new AttackEventArgs(direction));
  }

  async void Roll() {
    _speedMultiplier = 2f;

    await Task.Delay((int)(rollDuration * 1000));
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
    _onRoll += Roll;
  }
}