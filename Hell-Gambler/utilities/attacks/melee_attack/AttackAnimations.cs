using Godot;
using System;

public partial class AttackAnimations : Node, IEffect {
  [Export] string[] animationNames;
  [Export] AnimationPlayer animationPlayer;

  private int _animationIndex;
  void IEffect.Play() {
    animationPlayer.Play(animationNames[_animationIndex]);

    _animationIndex = (_animationIndex + 1) % animationNames.Length;
  }
}
