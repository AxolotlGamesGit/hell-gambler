using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class EnemyStateMachine : Node, IMovementInput {
  public Node2D Player = null;

  [ExportGroup("References")]
  [Export] Godot.Collections.Array<Node> stateNodes;
  System.Collections.Generic.Dictionary<int, IInputState> states = new();
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
  private event Action _onAttack;

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
  }

  async void Attack() {
    float _direction = (float)VectorMath.GetRotationFromVector((Player.Position - Parent.Position).Normalized());
    await attack.TryAttack(_direction);
  }

  public override void _EnterTree() {
    _onAttack += Attack;

    attack = attackNode as IAttack;

    for (int i = 0; i < stateNodes.Count; i++) {
      if (stateNodes[i] == null) {
        GD.PrintErr($"No node found at {i}");
      }
    }
    for (int i = 0; i < stateNodes.Count; i++) {
      if (stateNodes[i] is not IInputState) {
        GD.PrintErr($"No valid inputstate found at {i}");
      }
      states.Add(i, (IInputState)stateNodes[i]);
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

    if (states[(int)currentState].ShouldAttack()) {
      _onAttack?.Invoke();
    }
  }
}
