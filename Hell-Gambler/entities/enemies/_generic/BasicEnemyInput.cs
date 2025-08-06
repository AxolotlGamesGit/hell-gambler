using Godot;
using System;
using System.Threading.Tasks;

public partial class BasicEnemyInput : Node, IMovementInput, IAttackInput {
  public event EventHandler<AttackEventArgs> OnTryAttack;

  [ExportGroup("References")]
  [Export] Node2D parent;
  [Export] Node attackNode;
  IAttack attack;

  [ExportGroup("Parameters")]
  [Export] float idleDistance = 700;
  [Export] float attackDistance = 200;
  [Export] float attackDelaySeconds = 1;

  private enum State {
    Idle = 0,
    InRange = 1,
    Attacking = 2
  }
  private State _state = State.Idle;
  private Node2D _player = null;
  private event Action _onStartAttack;

  Vector2 IMovementInput.GetMoveInput() {
    if (_state == State.InRange) {
      return (_player.Position - parent.Position).Normalized();
    }
    else {
      return Vector2.Zero;
    }
  }

  async void TryAttack() {
    await Task.Delay((int)(attackDelaySeconds * 1000));

    if (IsInstanceValid(this)) {
      double _attackDirection = VectorMath.GetRotationFromVector(_player.Position - parent.Position);
      OnTryAttack?.Invoke(this, new AttackEventArgs((float)_attackDirection));
    }
  }

  public override void _EnterTree() {
    if (parent == null) {
      parent = (Node2D) GetParent();
    }
    if (attackNode == null) {
      attackNode = parent.GetNode("Attack");
    }
    attack = (IAttack) attackNode;

    _onStartAttack += TryAttack;
  }

  public override void _Ready() {
    if (_player == null) {
      _player = GetNode<CharacterBody2D>("/root/Game/Player");
    }
    if (_player == null) {
      GD.PrintErr("Player not found");
    }
  }

  public override void _Process(double delta) {
    float _distanceToPlayer = (parent.Position - _player.Position).Length();

    // Determines state
    if (_distanceToPlayer > idleDistance) {
      _state = State.Idle;
    }
    else if (attack.IsAttacking()) {
      _state = State.Attacking;
    }
    else {
      _state = State.InRange;
    }

    // Attacks if we are close enough and not currently attacking.
    if (_state == State.InRange  &&  _distanceToPlayer < attackDistance) {
      _onStartAttack?.Invoke();
    }
  }
}
