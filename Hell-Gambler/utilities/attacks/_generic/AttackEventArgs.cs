using Godot;
using System;

public class AttackEventArgs {
  public float AttackDirection;
  public AttackEventArgs(float attackDirection) {
    AttackDirection = attackDirection;
  }
}
