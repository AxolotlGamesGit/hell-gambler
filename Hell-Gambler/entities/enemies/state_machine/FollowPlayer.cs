using Godot;
using System;

public partial class FollowPlayer : Node, IInputBehavior {
  public event EventHandler<AttackEventArgs> OnTryAttack;

  [ExportGroup("References")]
  [Export] Node parentNode;
  EnemyStateMachine parent;

  [ExportGroup("Paramters")]
  [Export] bool shouldAttack = true;
  [Export] float attackRange = 100;
  [Export] float attackDelay = 1;

  Node2D player;

  Vector2 IMovementInput.GetMoveInput() {
    return (player.Position - parent.Parent.Position).Normalized();
  }

  void IBehavior.OnActivate() {
    player = parent.Player;
  }

  void IBehavior.OnDeactivate() {

  }

  public override void _EnterTree() {
    if (parentNode == null) {
      parentNode = GetParent();
    }
    parent = (EnemyStateMachine)parentNode;
  }

  public override void _Process(double delta) {
    if ((player.Position - parent.Parent.Position).Length() < attackRange) {
      OnTryAttack?.Invoke(this, new AttackEventArgs(0f));
    }
  }
}
