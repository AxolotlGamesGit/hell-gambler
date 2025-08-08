using Godot;
using System;
using System.Threading.Tasks;

public partial class MeleeAttack : Node, IAttack {
  [ExportGroup("References")]
  [Export] public MeleeAttackStats stats;
  [Export] Node2D parent;
  [Export] Node inputNode;
  [Export] PackedScene hitboxReference;
  IAttackInput input;

  [ExportGroup("Parameters")]
  [Export] bool followParent;
  [Export] bool damagesPlayer;

  private float _timeSinceLastAttack;
  private AttackHitbox _hitbox;
  private IEffect[] _animations;
  private int _animationIndex = 0;

  async Task IAttack.TryAttack(float direction) {
    IAttack self = this;
    if (self.CanAttack() == false) {
      return;
    }

    _timeSinceLastAttack = 0;
    _hitbox.Rotation = direction; // Needs to be before thread.sleep, very sketchy fix, otherwise collision detection will use old rotation value.

    await Task.Delay((int) (stats.StartDelay * 1000));

    _hitbox.Attack();

    if (_animations.Length >= 1  &&  _animations[0] != null) {
      _animations[_animationIndex].Play();

      _animationIndex = (_animationIndex + 1) % _animations.Length; ;
    }
  }

  bool IAttack.CanAttack() {
    return _timeSinceLastAttack >= stats.Cooldown;
  }

  bool IAttack.IsAttacking() {
    return _timeSinceLastAttack < stats.Duration;
  }

  async void TryAttackVoid(object sender, AttackEventArgs e) {
    IAttack self = this;
    await self.TryAttack(e.AttackDirection);
  }

  public override void _EnterTree() {
    SetProcess(true);

    if (hitboxReference == null) {
      hitboxReference = (PackedScene)ResourceLoader.Load("res://entities/attacks/_melee_attack/hitbox.tscn");
    }

    _hitbox = (AttackHitbox) hitboxReference.Instantiate();
    AddChild(_hitbox);
    _hitbox.Damage = stats.Damage;
    _hitbox.DamagesPlayer = damagesPlayer;
    for (int i = 0; i < stats.Hitboxes.Length; i ++) {
      CollisionShape2D collisionShape = new CollisionShape2D();
      collisionShape.Shape = stats.Hitboxes[i];
      collisionShape.Position = stats.HitboxOffsets[i];
      _hitbox.AddChild(collisionShape);
    }

    _animations = new IEffect[stats.Animations.Length];
    for (int i = 0; i < stats.Animations.Length; i++) {
      Node animation = stats.Animations[i].Instantiate();
      AddChild(animation);
      _animations[i] = (IEffect) animation;
    }
  }

  public override void _Ready() {
    if (inputNode == null) {
      inputNode = GetParent().GetNode("Input");
      if (inputNode == null) {
        GD.PrintErr("No valid input found");
      }
    }
    input = (IAttackInput) inputNode;
    input.OnTryAttack += TryAttackVoid;
  }

  public override void _Process(double delta) {
    _timeSinceLastAttack += (float) delta;
    if (followParent == true) {
      _hitbox.Position = parent.Position;
    }
  }
}
