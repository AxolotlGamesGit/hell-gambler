using Godot;
using System;

public enum AttackType {
  melee = 0
}
public partial class EnemyStats : Resource {
  [ExportGroup("Attack")]
  [Export] public AttackType AttackType;
  [Export] public Resource AttackStats;

  [ExportGroup("Stats")]
  [Export] public int Health;
  [Export] public float MoveSpeed;

  [ExportGroup("Behavior")]
  [Export] public PackedScene[] Behaviors = new PackedScene[3];

  [ExportGroup("Art")]
  [Export] public Texture2D Texture;
}
