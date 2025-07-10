using Godot;
using System;
using System.Collections.Generic;
using static Godot.PhysicsServer3D;

public partial class AttackHitbox : Area2D {
  [Export] bool DamagesPlayer = false;
  private List<Node2D> _bodies = new();

  public void Attack() {
    foreach (Node2D body in _bodies) {
      GD.Print("Damaged");
    }
  }

  private void OnBodyEntered(Node2D body) {
    _bodies.Add(body);
  }

  private void OnBodyExited(Node2D body) {
    _bodies.Remove(body);
  }

  public override void _EnterTree() {
    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;
  }

  public override void _ExitTree() {
    BodyEntered -= OnBodyEntered;
    BodyExited -= OnBodyExited;
  }
}
