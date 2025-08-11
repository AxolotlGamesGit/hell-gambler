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
  [Export] bool damagesPlayer;

  private float _timeSinceLastAttack;
  private AttackHitbox _hitbox;
  private Node _animationNode;
  private IEffect _animation;
  private int _animationIndex = 0;

  async Task TryAttack(float direction) {
    IAttack self = this;
    if (self.CanAttack() == false) {
      return;
    }

    _timeSinceLastAttack = 0;
    _hitbox.Rotation = direction; // Needs to be before thread.sleep, very sketchy fix, otherwise collision detection will use old rotation value.
    if (_animationNode is Node2D _node2D) {
      _node2D.Rotation = direction;
    }
    else {
      GD.Print("Animation is not a node 2d");
    }

    await Task.Delay((int) (stats.StartDelay * 1000));

    _hitbox.Attack();
    _animation?.Play();
  }

  bool IAttack.CanAttack() {
    return _timeSinceLastAttack >= stats.Cooldown;
  }

  bool IAttack.IsAttacking() {
    return _timeSinceLastAttack < stats.Duration;
  }

  async void TryAttackVoid(object sender, AttackEventArgs e) {
    await TryAttack(e.AttackDirection);
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

    if (stats.Animation != null) {
      _animationNode = stats.Animation.Instantiate();
      if (_animationNode is not IEffect) {
        GD.PrintErr($"Animation provided does not implement IEffect. Name: {this.Name}. Parent name: {parent.Name}");
        return;
      }
      AddChild(_animationNode);
      _animation = (IEffect)_animationNode;
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

    if (_animationNode is Node2D  && ((IAttack)this).IsAttacking() == false) {
      ((Node2D)_animationNode).Rotation = input.GetLookDirection();
    }
  }
}
