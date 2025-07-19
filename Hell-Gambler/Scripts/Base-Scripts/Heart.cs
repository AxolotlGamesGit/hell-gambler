using Godot;
using System;

public enum HeartStatus {
  alive = 0,
  dead = 1,
  empty = 2
}
public partial class Heart : Sprite2D {
  [Export] Sprite2D sprite;
  [Export] Texture2D alive;
  [Export] Texture2D dead;
  [Export] Texture2D empty;
  
  [Export] public HeartStatus Status = HeartStatus.alive;
  public Vector2 RelativePosition;
  public bool FollowParent;

  private Node2D parent;

  public override void _EnterTree() {
    SetProcess(true);
    Node _currentParent = GetParent();
    while (_currentParent is not Node2D) {
      _currentParent = _currentParent.GetParent();
    }
    parent = (Node2D) _currentParent;

    if (sprite == null) {
      sprite = this;
    }
  }

  public override void _Process(double delta) {
    switch (Status) {
      case HeartStatus.alive:
        sprite.Texture = alive;
        break;
      case HeartStatus.dead:
        sprite.Texture = empty;
        break;
      case HeartStatus.empty:
        sprite.Texture = dead;
        break;
    }

    if (FollowParent == true) {
      GlobalPosition = parent.Position + RelativePosition;
    }
    else {
      Position = RelativePosition;
    }
  }
}
