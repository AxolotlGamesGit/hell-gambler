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
  [Export] public float TopSpeed = 100f;
  [Export] float friction = 0.9f;

  public override void _EnterTree() {
    if (characterBody == null) {
      GD.Print($"{debugTag}: character body not provided.");
    }

    characterBody.PlatformFloorLayers = new();
  }

  public override void _Ready() {
    if (inputNode == null) {
      inputNode = GetParent().GetNode("Input");
      if (inputNode == null) {
        GD.PrintErr("No valid input found");
      }
    }
    input = (IMovementInput)inputNode;

    SetPhysicsProcess(true);
  }

  public override void _PhysicsProcess(double delta) {
    Vector2 moveInput = input.GetMoveInput();

    float acceleration = 0;
    if (friction < 1) {
      acceleration = ((1f / friction) - 1f) * TopSpeed;    // Magic formula found at https://www.desmos.com/calculator/qkzwobcwyk
    }
    else {
      acceleration = TopSpeed - characterBody.Velocity.Length();
    }

    Vector2 _velocity = characterBody.Velocity;
    _velocity += moveInput * acceleration;
    _velocity *= friction;
    characterBody.Velocity = _velocity;
    characterBody.MoveAndSlide();
  }
}
