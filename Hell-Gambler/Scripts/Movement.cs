using Godot;
using System;
using System.Diagnostics;

public partial class Movement : Node {
  [Export] string debugTag;

  [ExportGroup("References")]
  [Export] CharacterBody2D characterBody;
  [Export] Node inputNode;
  private  IMovementInput input;

  [ExportGroup("Parameters")]
  [Export] float topSpeed = 3.0f;
  [Export] float friction = 0.9f;

  public override void _EnterTree() {
    if (characterBody == null) {
      GD.Print($"{debugTag}: character body not provided.");
    }

    input = (IMovementInput) inputNode;

    characterBody.PlatformFloorLayers = new();
  }

  public override void _Ready() {
    SetPhysicsProcess(true);
  }

  public override void _PhysicsProcess(double delta) {
    Vector2 moveInput = input.GetInput();

    float acceleration = 0;
    if (friction < 1) {
      acceleration = ((1f / friction) - 1f) * topSpeed;    // Magic formula found at https://www.desmos.com/calculator/qkzwobcwyk
    }
    else {
      acceleration = topSpeed - characterBody.Velocity.Length();
    }

    Vector2 _velocity = characterBody.Velocity;
    _velocity += moveInput * acceleration;
    _velocity *= friction;
    characterBody.Velocity = _velocity;
    characterBody.MoveAndSlide();
  }
}
