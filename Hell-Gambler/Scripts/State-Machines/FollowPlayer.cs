using Godot;
using System;

public partial class FollowPlayer : Node, IInputState {
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

  bool IInputState.ShouldAttack() {
    if ((player.Position - parent.Parent.Position).Length() < attackRange) {
      return shouldAttack;
    }
    
    return false;
  }

  void IState.OnActivate() {
    player = parent.Player;
  }

  void IState.OnDeactivate() {

  }

  public override void _EnterTree() {
    if (parentNode == null) {
      parentNode = GetParent();
    }
    parent = (EnemyStateMachine)parentNode;
  }
}
