namespace CeresECL.Example
{
	public class BulletEntity : Entity
	{
		protected override void Build()
		{
			Logics.Add<MoveLogic>();
			Logics.Add<BulletLogic>();
		}
	}
}