using System;
using System.Threading.Tasks;

public interface IAttack {
  public bool CanAttack();
  public bool IsAttacking();
}
