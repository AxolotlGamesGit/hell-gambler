using Godot;
using System;

public interface IInputState : IState, IMovementInput {
  bool ShouldAttack();
}
