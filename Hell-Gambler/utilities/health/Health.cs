using Godot;
using System;
using System.Collections.Generic;

public partial class Health : Node {
  public int CurrentHealth { get; private set; }
  public event Action OnDeath;
  [Export] public int MaxHealth = 10;

  [ExportGroup("References")]
  [Export] PackedScene heart;
  [Export] Node parent;

  [ExportGroup("Behavior")]
  [Export] bool destroyOnDeath = true;

  [ExportGroup("SpawningParameters")]
  [Export] Vector2 startingLocation = new Vector2(0, -55);
  [Export] float xOffset = 20;
  [Export] float size = 0.4f;
  [Export] bool followParent = true;

  private List<Heart> _hearts;

  public void Damage(int damage) {
    CurrentHealth -= damage;
    DisplayHealth();

    if (CurrentHealth <= 0) {
      OnDeath?.Invoke();
    }
  }

  void DisplayHealth() {
    for (int i = 0; i < _hearts.Count; i++) {
      if (i + 1 > MaxHealth) {
        _hearts[i].Status = HeartStatus.empty;
      }
      else if (i + 1 > CurrentHealth) {
        _hearts[i].Status = HeartStatus.dead;
      }
      else {
        _hearts[i].Status = HeartStatus.alive;
      }
    }
  }

  public override void _EnterTree() {
    _hearts = new List<Heart>();
    Vector2 _currentLocation = startingLocation;
    for (int i = 0; i < MaxHealth; i++) {
      Heart _heart = (Heart) heart.Instantiate();
      _heart.RelativePosition = _currentLocation;
      _heart.Scale = new Vector2(size, size);
      _heart.FollowParent = followParent;
      this.CallDeferred("add_child", _heart);
      _hearts.Add(_heart);
      _currentLocation.X += xOffset;
    }

    CurrentHealth = MaxHealth;
  }

  public override void _Ready() {
    DisplayHealth();

    if (destroyOnDeath) { 
      OnDeath += parent.Free;
    }
  }
}