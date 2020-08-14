namespace CeresECL.Example
{
	public class EnemyEntity : Entity
	{
		protected override void Build()
		{
			Logics.Add<MoveLogic>();
		}
	}
}