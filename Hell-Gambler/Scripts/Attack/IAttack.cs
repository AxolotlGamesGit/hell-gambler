using System.Threading.Tasks;

public interface IAttack {
  public bool CanAttack();
  public async Task TryAttack(float direction) { }
}
