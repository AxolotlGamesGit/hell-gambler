using Godot;
using System;

public interface IState {
  public void OnActivate();
  public void OnDeactivate();
}
