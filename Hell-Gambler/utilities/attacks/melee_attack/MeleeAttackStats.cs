using Godot;
using System;

public partial class MeleeAttackStats : Resource {
  [ExportGroup("Effects")]
  [Export] public PackedScene Animation;
  [Export] public Shape2D[] Hitboxes = [];
  [Export] public Vector2[] HitboxOffsets = [];

  [ExportGroup("Stats")]
  [Export] public int Damage = 1;
  [Export] public float Cooldown = 2f;
  [Export] public float Duration = 1;
  [Export] public float StartDelay = 0.2f;
}
