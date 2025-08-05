using Godot;
using System;

public partial class Enemy : CharacterBody2D {
  [Export] EnemyStats stats;

  [ExportGroup("References")]
  [Export] Movement movement;
  [Export] Health health;
  [Export] Node attackNode;
  [Export] Sprite2D sprite;

  public override void _EnterTree() {
    Node _input = stats.Input.Instantiate();
    _input.Name = "Input";
    AddChild(_input);

    movement.TopSpeed = stats.MoveSpeed;

    health.MaxHealth = stats.Health;
    
    if (stats.AttackType == AttackType.melee) {
      ((MeleeAttack)attackNode).stats = (MeleeAttackStats)stats.AttackStats;
    }

    sprite.Texture = stats.Texture;
  }

  public override void _Ready() {
  }
}
