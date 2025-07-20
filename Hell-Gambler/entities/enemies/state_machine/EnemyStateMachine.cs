using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class EnemyStateMachine : Node, IMovementInput, IAttackInput {
  public Node2D Player = null;
  public event EventHandler<AttackEventArgs> OnTryAttack;

  [ExportGroup("References")]
  [Export] Godot.Collections.Array<Node> stateNodes;
  System.Collections.Generic.Dictionary<int, IInputBehavior> states = new();
  [Export] public Node2D Parent;
  [Export] Node attackNode;
  IAttack attack;

  [ExportGroup("Parameters")]
  [Export] State currentState = State.Idle;
  [Export] float idleDistance = 700;

  private enum State {
    Idle = 0,
    InRange = 1,
    Attacking = 2
  }

  Vector2 IMovementInput.GetMoveInput() {
    return states[(int)currentState].GetMoveInput();
  }

  void TransitionLogic() {
    switch (currentState) {
      case State.Idle:
        if ((Parent.Position - Player.Position).Length() < idleDistance) {
          SwitchToState(State.InRange);
        }
        break;
      case State.InRange:
        if ((Parent.Position - Player.Position).Length() > idleDistance) {
          SwitchToState(State.Idle);
        }
        if (attack.IsAttacking() == true) {
          SwitchToState(State.Attacking);
        }
        break;
      case State.Attacking:
        if (attack.IsAttacking() == false) {
          SwitchToState(State.InRange);
        }
        break;
    }
  }

  void SwitchToState(State state) {
    states[(int) currentState].OnDeactivate();
    currentState = state;
    states[(int) currentState].OnActivate();
    states[(int)currentState].OnTryAttack += TryAttack;
  }

  void TryAttack(object sender, AttackEventArgs e) {
    float _direction = (float)VectorMath.GetRotationFromVector((Player.Position - Parent.Position).Normalized());
    AttackEventArgs _eventArgs = new(_direction);
    OnTryAttack?.Invoke(this, _eventArgs);
  }

  public override void _EnterTree() {
    attack = attackNode as IAttack;

    for (int i = 0; i < stateNodes.Count; i++) {
      if (stateNodes[i] == null) {
        GD.PrintErr($"No node found at {i}");
      }
    }
    for (int i = 0; i < stateNodes.Count; i++) {
      if (stateNodes[i] is not IInputBehavior) {
        GD.PrintErr($"No valid inputstate found at {i}");
      }
      states.Add(i, (IInputBehavior)stateNodes[i]);
    }
  }

  public override void _Ready() {
    if (Player == null) {
      Player = GetNode<CharacterBody2D>("/root/Game/Player");
    }
    if (Player == null) {
      GD.PrintErr("Man fuh this bullshi");
    }
  }

  public override void _Process(double delta) {
    TransitionLogic();
  }
}
