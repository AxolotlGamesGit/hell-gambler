using Godot;
using System;

public partial class EnemyInput : Node, IMovementInput {
  [ExportGroup("References")]
  [Export] CharacterBody2D player;
  [Export] CharacterBody2D self;

  Vector2 IMovementInput.GetInput() {
		if (player == null) {
		  GD.Print("player reference not set");
		}
		if (self == null) {
			GD.Print("self reference not set");
		}
		return (player.Position - self.Position).Normalized();
  }

  public override void _EnterTree() {
	  if (player == null) {
		  player = GetNode<CharacterBody2D>("/root/Game/Player");
	  }
  }
}
