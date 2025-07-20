using System;
using System.Threading.Tasks;

public interface IAttack {
  public bool CanAttack();
  public bool IsAttacking();
  public async Task TryAttack(float direction) { await Task.Delay(1); }
}
