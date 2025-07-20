using Godot;
using System;
using System.Threading.Tasks;

public partial class FollowPlayer : Node, IInputBehavior {
  public event EventHandler<AttackEventArgs> OnTryAttack;

  [ExportGroup("References")]
  [Export] Node stateMachineNode;
  EnemyStateMachine stateMachine;

  [ExportGroup("Paramters")]
  [Export] bool shouldAttack = true;
  [Export] float attackRange = 100;

  private Node2D player;

  Vector2 IMovementInput.GetMoveInput() {
    return (player.Position - stateMachine.Parent.Position).Normalized();
  }

  void IBehavior.OnActivate() {
    player = stateMachine.Player;
  }

  void IBehavior.OnDeactivate() {

  }

  public override void _EnterTree() {
    if (stateMachineNode == null) {
      stateMachineNode = GetParent();
    }
    stateMachine = (EnemyStateMachine)stateMachineNode;
  }

  public override void _Process(double delta) {
    if ((player.Position - stateMachine.Parent.Position).Length() < attackRange) {
      OnTryAttack?.Invoke(this, new AttackEventArgs(0f));
    }
  }
}
