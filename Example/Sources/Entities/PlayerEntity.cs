namespace CeresECL.Example
{
    public class PlayerEntity : Entity
    {
		protected override void Build()
        {
           Logics.Add<InputLogic>();
           Logics.Add<MoveLogic>();
        }
    }
}