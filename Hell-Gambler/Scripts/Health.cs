using Godot;
using System;
using System.Collections.Generic;

public partial class Health : Node {
  [ExportGroup("References")]
  [Export] PackedScene heart;
  [Export] Node parent;

  [ExportGroup("Parameters")]
  [Export] int maxHealth = 10;
  [Export] Vector2 startingLocation = new Vector2(0, -55);
  [Export] float xOffset = 20;
  [Export] float size = 0.4f;
  [Export] bool followParent = true;

  [Export] public int CurrentHealth { get; private set; }
  public event EventHandler OnDeath;

  private List<Heart> _hearts;

  public void AddHealth(int health) {
    CurrentHealth += health;
    DisplayHealth();

    if (health < 0) {
      OnDeath.Invoke(this, new());
    }
  }

  void DisplayHealth() {
    for (int i = 0; i < _hearts.Count; i++) {
      if (i + 1 > maxHealth) {
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
    for (int i = 0; i < maxHealth; i++) {
      Heart _heart = (Heart) heart.Instantiate();
      _heart.RelativePosition = _currentLocation;
      _heart.Scale = new Vector2(size, size);
      _heart.FollowParent = followParent;
      parent.CallDeferred("add_child", _heart);
      _hearts.Add(_heart);
      _currentLocation.X += xOffset;
    }

    CurrentHealth = maxHealth;
  }

  public override void _Ready() {
    DisplayHealth();
  }

  public override void _Process(double delta) {
    //DisplayHealth();
  }
}