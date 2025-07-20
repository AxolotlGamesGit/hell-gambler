using Godot;
using System;

public partial class Enemy : CharacterBody2D {
  [Export] EnemyStats stats;

  [ExportGroup("References")]
  [Export] EnemyStateMachine stateMachine;
  [Export] Movement movement;
  [Export] Health health;
  [Export] Node attackNode;
  [Export] Sprite2D sprite;

  public override void _EnterTree() {
    for (int i = 0; i < 3; i++) {
      Node _behavior = stats.Behaviors[i].Instantiate();
      stateMachine.AddChild(_behavior);
      stateMachine.StateNodes[i] = _behavior;
    }

    movement.TopSpeed = stats.MoveSpeed;

    health.MaxHealth = stats.Health;
    
    if (stats.AttackType == AttackType.melee) {
      ((MeleeAttack)attackNode).stats = (MeleeAttackStats)stats.AttackStats;
    }

    sprite.Texture = stats.Texture;
  }
}
