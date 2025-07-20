using Godot;
using System;

public interface IAttackInput {
  public event EventHandler<AttackEventArgs> OnTryAttack;
}
