namespace CeresECL.Example
{
	public class EnemyEntityBuilder : Builder
	{
		protected override void Build()
		{
			Entity.Logics.Add<MoveLogic>();
		}
	}
}