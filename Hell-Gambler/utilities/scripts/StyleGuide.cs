using Godot;
using System;

public partial class StyleGuide : Node, IEffect {
  public int PublicVariable = 0; // PascalCase for public variables only

  [Export] int exportedVariable = 0; // camelCase for exported variables and references
  [Export] Node attackNode;          // references to abstractions should have a node reference suffixed with node
  private IAttack attack;            // the actual reference should be in camelCase because it is just a cast from an exported reference

  private int _privateVariable;      // _camelCase for private variables

  public void PublicMethod(int parameter) {           // parameters should be camelCase because they are not being modified, similar to exported variables
    MeleeAttack _meleeAttack = (MeleeAttack) attack;  // all local variables, including references, should be _camelCase
  }

  void IEffect.Play() {

  }

  void PrivateMethod() {

  }

  public override void _EnterTree() {
    attack = (IAttack) attackNode;        // use parenthesis and a space for casts
    _privateVariable = exportedVariable;  // do all setup i8n _EnterTree
  }

  public override void _Ready() {
    if (attack.IsAttacking()) {    // Don't mess with references untill _Ready because you need to wait for them to set up in _EnterTree
      PrivateMethod();
    }
  }

  public override void _Process(double delta) {

  }

  public override void _PhysicsProcess(double delta) {

  }
}
