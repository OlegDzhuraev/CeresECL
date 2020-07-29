namespace CeresECL.Example
{
	public class BulletEntityBuilder : Builder
	{
		protected override void Build()
		{
			Entity.Logics.Add<MoveLogic>();
			Entity.Logics.Add<BulletLogic>();
		}
	}
}