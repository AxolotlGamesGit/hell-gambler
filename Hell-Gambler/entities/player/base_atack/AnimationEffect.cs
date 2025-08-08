using Godot;
using System;

public partial class AnimationEffect : AnimationPlayer, IEffect {
  [Export] string animationName;
  void IEffect.Play() {
    this.Play(animationName);
  }
}
