using Godot;
using System;

public partial class EnemyInput : Node, IMovementInput {
  [ExportGroup("References")]
  [Export] CharacterBody2D player;
  [Export] CharacterBody2D self;
  [Export] Node attackNode;
  private IAttack attack;

  [ExportGroup("Parameters")]
  [Export] float range = 100;

  private event Action _onAttack;

  Vector2 IMovementInput.GetMoveInput() {
		if (player == null) {
		  GD.Print("player reference not set");
		}
		if (self == null) {
			GD.Print("self reference not set");
		}
		return (player.Position - self.Position).Normalized();
  }

  async void Attack() {
    await attack.TryAttack((float) VectorMath.GetRotationFromVector((player.Position - self.Position).Normalized()));
  }

  public override void _EnterTree() {
	  if (player == null) {
		  player = GetNode<CharacterBody2D>("/root/Game/Player");
	  }

    attack = (IAttack) attackNode;

    _onAttack += Attack;
  }

  public override void _Process(double delta) {
    if ((player.Position - self.Position).Length() < range  &&  attack.CanAttack()) {
      _onAttack.Invoke();
    }
  }
}
