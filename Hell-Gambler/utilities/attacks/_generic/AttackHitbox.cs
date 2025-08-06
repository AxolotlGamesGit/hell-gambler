using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Godot.PhysicsServer3D;

public partial class AttackHitbox : Area2D {
  [Export] public int Damage = 1;
  [Export] public bool DamagesPlayer = false;
  private List<Node2D> _bodies = new();

  private void AttackBody(Node2D body) {
    if (body.HasNode("%Health")) {
      if (DamagesPlayer ^ body.HasNode("%PlayerInput") == false) {
        Health health = body.GetNode<Health>("%Health");
        health.Damage(Damage);
      }
    }
  }

  public void Attack() {
    for (int i = _bodies.Count - 1; i > 0; i--) {
      if (_bodies[i] != null) {
        AttackBody(_bodies[i]);
      }
      else {
        _bodies.Remove(_bodies[i]);
      }
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

    _bodies.Clear();
  }
}
