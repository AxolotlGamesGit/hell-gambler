using Godot;
using System;
using System.Threading;

public partial class MeleeAttack : Node, IAttack {
  [Export] MeleeAttackStats stats;
  [Export] Node2D parent;
  [Export] bool followParent;

  private float _timeSinceLastAttack;
  private PackedScene _hitboxReference = (PackedScene) ResourceLoader.Load("res://Objects/Hitbox.tscn");
  private AttackHitbox _hitbox;

  void IAttack.TryAttack(float direction) {
    IAttack self = this;
    if (self.CanAttack() == false) {
      GD.Print($"Can't attack. Time since last attack: {_timeSinceLastAttack}");
      return;
    }

    _hitbox.Rotation = direction; // Needs to be before thread.sleep, very sketchy fix, otherwise collision detection will use old rotation value.

    Thread.Sleep((int) (1000 * stats.StartDelay));

    _hitbox.Attack();
    foreach (IEffect effect in stats.Effects) {
      effect.Play();
    }
    _timeSinceLastAttack = 0;
    GD.Print("Attacked");
  }

  bool IAttack.CanAttack() {
    return _timeSinceLastAttack >= stats.Cooldown;
  }

  public override void _EnterTree() {
    SetProcess(true);

    _hitbox = (AttackHitbox) _hitboxReference.Instantiate();
    AddChild(_hitbox);
    for (int i = 0; i < stats.Hitboxes.Length; i ++) {
      CollisionShape2D collisionShape = new CollisionShape2D();
      collisionShape.Shape = stats.Hitboxes[i];
      collisionShape.Position = stats.HitboxOffsets[i];
      _hitbox.AddChild(collisionShape);
    }
  }

  public override void _Process(double delta) {
    _timeSinceLastAttack += (float) delta;
    if (followParent == true) {
      _hitbox.Position = parent.Position;
    }
  }
}
